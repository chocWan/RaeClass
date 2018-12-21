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
    public class FormContentRepository
    {
        public static RaeClassContentType raeClassContentType = RaeClassContentType.Read;
        private RaeClassContext context;
        private ISerialNumberRepository serialNumberRepository;
        public FormContentRepository(RaeClassContext _context, ISerialNumberRepository _serialNumberRepository)
        {
            context = _context;
            serialNumberRepository = _serialNumberRepository;
        }

        public async Task<int> AddAsync(string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
        {
            Read read = GetEmptyFormContent(level, name, cncontent, encontent, recordFileId1, recordFileId2);
            ReadContent content = GetReadContent(read);
            context.ReadContentSet.Add(content);
            int res = await context.SaveChangesAsync();
            if (res == 1) serialNumberRepository.UpdateMaxIndex(raeClassContentType);
            return res;
        }

        public Tuple<List<Read>, int> GetPageListAsync(int pageindex, int pagesize, string level, string titleOrContent)
        {
            List<Read> reads = new List<Read>();
            int pagecount = 0;
            StringBuilder sb = new StringBuilder();
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
            countSb.Append("select FId from ReadContent where 1=1 ");
            countSb.Append(sb.ToString());
            int count = context.ReadContentSet.FromSql(countSb.ToString()).Count();
            if (count >= 0)
            {
                StringBuilder contentSb = new StringBuilder();
                contentSb.Append("select FJsonData from ReadContent where 1=1 ");
                contentSb.Append(sb.ToString());
                contentSb.AppendFormat(" limit {0} offset {1} ", pagesize, pageindex);
                List<string> jsonDatas = context.Set<ReadContent>().Select(x => x.FJsonData).FromSql(contentSb.ToString()).ToList();
                reads = JsonHelper.ConvertToModelList<Read>(jsonDatas);
                pagecount = count % pagesize == 0 ? count / pagesize : count / pagesize + 1;
            }

            return new Tuple<List<Read>, int>(reads, count);
        }

        public Task<int> UpdateAsync(string readNumber, string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
        {
            var read = GetRead(readNumber);
            read.flevel = level;
            read.fname = name;
            read.fcnContent = cncontent;
            read.fenContent = encontent;
            read.frecordFileId1 = recordFileId1;
            read.frecordFileId2 = recordFileId2;
            read.fmodifyTime = DateTime.Now.ToString();
            read.fmodifyBy = "Rae";
            var query = context.ReadContentSet.Where(x => x.FNumber.Equals(readNumber)).FirstOrDefault();
            if (query != null)
            {
                query.FName = read.fname;
                query.FLevel = read.flevel;
                query.FJsonData = JsonHelper.SerializeObject(read);
                query.FModifyTime = DateTime.Now;
            }
            return context.SaveChangesAsync();
        }

        public ReadContent GetReadContent(Read read)
        {
            ReadContent readContent = new ReadContent();
            readContent.FNumber = read.fnumber;
            readContent.FName = read.fname;
            readContent.FLevel = read.flevel;
            readContent.FJsonData = JsonHelper.SerializeObject(read);
            readContent.FCreateTime = DateTime.Now;
            readContent.FModifyTime = DateTime.Now;
            return readContent;
        }

        public Read GetRead(string readNumber)
        {
            var query = context.ReadContentSet.Where(x => x.FNumber.Equals(readNumber)).FirstOrDefault();
            if (query != null) return JsonHelper.ConvertToModel<Read>(query.FJsonData);
            else return null;
        }

        public FormContent GetEmptyFormContent(string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
        {
            FormContent formContent = new FormContent();
            formContent._id = string.Empty;
            formContent._openid = Const.WX_OPENID;
            formContent.flevel = level;
            formContent.fnumber = serialNumberRepository.GetSerialNumber(raeClassContentType);
            formContent.fname = name;
            formContent.fcnContent = cncontent;
            formContent.fenContent = encontent;
            formContent.fcreateTime = DateTime.Now.ToString();
            formContent.fcreateBy = "Rae";
            formContent.fmodifyTime = DateTime.Now.ToString();
            formContent.fmodifyBy = "Rae";
            formContent.fdocStatus = "C";
            formContent.frecordFileId1 = Const.WX_READ_RECORD_PREFIX + recordFileId1;
            formContent.frecordFileId2 = Const.WX_READ_RECORD_PREFIX + recordFileId2;
            return formContent;
        }

    }
}
