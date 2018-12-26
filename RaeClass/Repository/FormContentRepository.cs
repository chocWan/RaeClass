using Microsoft.EntityFrameworkCore;
using RaeClass.Config;
using RaeClass.Helper;
using RaeClass.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaeClass.Repository
{
    public class FormContentRepository: IFormContentRepository
    {
        
        private RaeClassContext context;
        private ISerialNumberRepository serialNumberRepository;
        public FormContentRepository(RaeClassContext _context, ISerialNumberRepository _serialNumberRepository)
        {
            context = _context;
            serialNumberRepository = _serialNumberRepository;
        }

        public async Task<FormContent> AddAsync(RaeClassContentType contentType,FormContent formContent)
        {
            FillFormContent(contentType,ref formContent);
            BaseFormContent baseFormContent = GetBaseFormContent(contentType, formContent);
            context.BaseFormContentSet.Add(baseFormContent);
            int res = await context.SaveChangesAsync();
            if (res == 1)
            {
                serialNumberRepository.UpdateMaxIndex(contentType);
                return formContent;
            }
            else {
                return null;
            }
        }

        public async Task<int> AddListAsync(RaeClassContentType contentType, List<FormContent> formContents)
        {
            FillFormContents(contentType, ref formContents);
            List<BaseFormContent> baseFormContents = GetBaseFormContents(contentType, formContents);
            await context.BaseFormContentSet.AddRangeAsync(baseFormContents);
            int res = await context.SaveChangesAsync();
            if (res == 1) serialNumberRepository.UpdateMaxIndex(contentType);
            return res;
        }

        public Task<int> UpdateAsync(FormContent formContent)
        {
            UpdateBaseFormContent(formContent);
            return context.SaveChangesAsync();
        }

        private void UpdateBaseFormContent(FormContent formContent)
        {
            formContent.fmodifyTime = DateTime.Now.ToString();
            formContent.fmodifyBy = CONST.CREATOR;
            var query = context.BaseFormContentSet.Where(x => x.FNumber.Equals(formContent.fnumber)).FirstOrDefault();
            if (query != null)
            {
                query.FName = formContent.fname;
                query.FLevel = formContent.flevel;
                query.FJsonData = JsonHelper.SerializeObject(formContent);
                query.FModifyTime = DateTime.Now;
                query.FDocStatus = formContent.fdocStatus;
            }
        }

        public Task<int> UpdateListAsync(List<FormContent> formContents)
        {
            formContents.ForEach(item=>UpdateBaseFormContent(item));
            return context.SaveChangesAsync();
        }

        public async Task<int> UpdateDocStatusListAsync(List<string> fnumbers,string docStatus)
        {

            await context.BaseFormContentSet
                .Where(x => fnumbers.Contains(x.FNumber))
                .ForEachAsync( item=> {
                    var formContent = GetFormContent(item.FNumber);
                    formContent.fdocStatus = docStatus;
                    formContent.fmodifyTime = DateTime.Now.ToString();
                    formContent.fmodifyBy = CONST.CREATOR;
                    item.FJsonData = JsonHelper.SerializeObject(formContent);
                    item.FModifyTime = DateTime.Now;
                });


            return await context.SaveChangesAsync();
        }

        public Tuple<List<FormContent>, int> GetPageListAsync(RaeClassContentType contentType, string level, string titleOrContent, int pageindex, int pagesize)
        {
            List<FormContent> contents = new List<FormContent>();
            int pagecount = 0;
            StringBuilder sb = new StringBuilder();
            if (contentType != 0)
            {
                sb.AppendFormat(" and FContentType = '{0}' ", contentType);
            }
            if (level.Trim() != "0")
            {
                sb.AppendFormat(" and json_extract(FJsonData,\"$.flevel\") = '{0}' ", level);
            }
            if (!string.IsNullOrEmpty(titleOrContent))
            {
                sb.Append(" and (");
                sb.AppendFormat(" json_extract(FJsonData,\"$.fname\") like '%{0}%' ", titleOrContent);
                sb.Append(" or");
                sb.AppendFormat(" json_extract(FJsonData,\"$.fcnContent\") = '%{0}%' ", titleOrContent);
                sb.Append(" or");
                sb.AppendFormat(" json_extract(FJsonData,\"$.fenContent\") = '%{0}%' ", titleOrContent);
                sb.Append(" )");
            }
            StringBuilder countSb = new StringBuilder();
            countSb.Append("select FId from BaseFormContent where 1=1 ");
            countSb.Append(sb.ToString());
            int count = context.BaseFormContentSet.FromSql(countSb.ToString()).Count();
            if (count >= 0)
            {
                StringBuilder contentSb = new StringBuilder();
                contentSb.Append("select FJsonData from BaseFormContent where 1=1 ");
                contentSb.Append(sb.ToString());
                contentSb.AppendFormat(" limit {0} offset {1} ", pagesize, pageindex);
                List<string> jsonDatas = context.Set<BaseFormContent>().Select(x => x.FJsonData).FromSql(contentSb.ToString()).ToList();
                contents = JsonHelper.ConvertToModelList<FormContent>(jsonDatas);
                pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            }

            return new Tuple<List<FormContent>, int>(contents, count);
        }

        public async Task<FormContent> GetFormContentAsync(string fnumber)
        {
            var query = await context.BaseFormContentSet.Where(x => x.FNumber.Equals(fnumber)).FirstOrDefaultAsync();
            if (query != null) return JsonHelper.ConvertToModel<FormContent>(query.FJsonData);
            else return null;
        }

        public FormContent GetFormContent(string fnumber)
        {
            var query = context.BaseFormContentSet.Where(x => x.FNumber.Equals(fnumber)).FirstOrDefault();
            if (query != null) return JsonHelper.ConvertToModel<FormContent>(query.FJsonData);
            else return null;
        }

        public async Task<List<FormContent>> GetFormContentListAsync(List<string> fnumbers)
        {
            var query = await context.BaseFormContentSet.Where(x => fnumbers.Contains(x.FNumber)).ToListAsync();
            if (query != null) return JsonHelper.ConvertToModelList<FormContent>(query.Select(x => x.FJsonData).ToList());
            else return new List<FormContent>();
        }

        public FormContent GetEmptyFormContent()
        {
            var formContent = new FormContent();
            formContent.fdocStatus = DocStatus.SAVE;
            return new FormContent(); 
        }

        private BaseFormContent GetBaseFormContent(RaeClassContentType contentType, FormContent formContent)
        {
            BaseFormContent baseFormContent = new BaseFormContent();
            baseFormContent.FContentType = contentType.ToString();
            baseFormContent.FDocStatus = DocStatus.SAVE;
            baseFormContent.FNumber = formContent.fnumber;
            baseFormContent.FName = formContent.fname;
            baseFormContent.FLevel = formContent.flevel;
            baseFormContent.FJsonData = JsonHelper.SerializeObject(formContent);
            baseFormContent.FCreateTime = DateTime.Now;
            baseFormContent.FModifyTime = DateTime.Now;
            return baseFormContent;
        }

        private List<BaseFormContent> GetBaseFormContents(RaeClassContentType contentType, List<FormContent> formContents)
        {
            List<BaseFormContent> baseFormContents = new List<BaseFormContent>();
            formContents.ForEach(item=> baseFormContents.Add(GetBaseFormContent(contentType,item)));
            return baseFormContents;
        }

        private void FillFormContent(RaeClassContentType contentType,ref FormContent formContent)
        {
            formContent._id = "";
            formContent._openid = CONST.WX_OPENID;
            formContent.fnumber = serialNumberRepository.GetSerialNumber(contentType);
            formContent.fcreateTime = DateTime.Now.ToString();
            formContent.fcreateBy = CONST.CREATOR;
            formContent.fmodifyTime = DateTime.Now.ToString();
            formContent.fmodifyBy = CONST.CREATOR;
            formContent.frecordFileId1 = CommonUtils.GetRecordFilePrefix(contentType) + formContent.frecordFileId1;
            formContent.frecordFileId2 = CommonUtils.GetRecordFilePrefix(contentType) + formContent.frecordFileId2;
        }

        private void FillFormContents(RaeClassContentType contentType, ref List<FormContent> formContents)
        {
            formContents.ForEach(item => FillFormContent(contentType,ref item));
        }

    }
}
