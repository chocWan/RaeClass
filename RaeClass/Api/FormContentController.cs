using Microsoft.AspNetCore.Mvc;
using RaeClass.Config;
using RaeClass.Helper;
using RaeClass.Models;
using RaeClass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaeClass.Api
{
    [Produces("application/json")]
    [Route("api/FormContent")]
    public class FormContentController : Controller
    {

        private IFormContentRepository formContentRepository;
        public FormContentController(IFormContentRepository _formContentRepository)
        {
            formContentRepository = _formContentRepository;
        }

        [HttpGet]
        public JsonResult Get(RaeClassContentType contentType,string level, string titleOrContent, int pageindex = 1, int pagesize = 10)
        {
            var res = formContentRepository.GetPageListAsync(contentType, level, titleOrContent, pageindex, pagesize);
            return Json(new { total = res.Item2, rows = res.Item1 });
        }

        [HttpGet("GetFormContent")]
        public async Task<JsonResult> GetFormContent(string fnumber)
        {
            return Json(new { content = await formContentRepository.GetFormContentAsync(fnumber) });
        }

        [HttpGet("GetFormContentList")]
        public async Task<JsonResult> GetEmptyFormContentList(List<string> fnumbers)
        {
            return Json(new { content = await formContentRepository.GetFormContentListAsync(fnumbers) });
        }

        [HttpGet("GetEmptyFormContent")]
        public async Task<JsonResult> GetEmptyFormContent()
        {
            return Json(new { content = await formContentRepository.GetEmptyFormContent() });
        }

        [HttpGet("GetArticlesByDate")]
        public JsonResult GetArticlesByDate(int dateGap)
        {
            return Json(new { content = formContentRepository.GetArticlesQtyByDate(dateGap) });
        }

        [HttpPost("Save")]
        public async Task<JsonResult> Save(RaeClassContentType contentType, FormContent formContent)
        {
            FormContent _formContent = null;
            if (string.IsNullOrEmpty(formContent.fnumber))
            {
                _formContent = await formContentRepository.AddAsync(contentType, formContent);
            }
            else {
                if (!formContent.fdocStatus.Equals(DocStatus.SAVE) && !formContent.fdocStatus.Equals(DocStatus.AUDIT))
                {
                    string msg = "只有保存或审核状态才允许保存，此文章不符合该条件！";
                    return Json(new { isok = false ,errmsg = msg });
                }
                formContent.fdocStatus = DocStatus.SAVE;
                _formContent = await formContentRepository.UpdateAsync(formContent);
            }

            if (_formContent != null) return Json(new { isok = true, content = _formContent });
            else return Json(new { isok = false });
        }

        [HttpPost("SubmitWithContent")]
        public async Task<JsonResult> Submit(RaeClassContentType contentType, List<FormContent> formContents)
        {
            formContents.ForEach(item=>item.fdocStatus = DocStatus.AUDIT);
            int querySaveStatusCount = formContents.Where(x => string.IsNullOrEmpty(x.fnumber)).Count();
            if (querySaveStatusCount > 0) throw new Exception("there are doc being save status,please save first!");

            var querySubmit = formContents.Where(x => string.IsNullOrEmpty(x.fnumber));
            int submitCount = await formContentRepository.UpdateListAsync(formContents);
            return Json(new { isok = true });
        }

        [HttpPost("Audit")]
        public async Task<JsonResult> Audit(List<string> fnumbers)
        {
            var query = await formContentRepository.GetFormContentListAsync(fnumbers);
            var res = query.Where(x=>!x.fdocStatus.Equals(DocStatus.SAVE)).Select(x=>x.fnumber).ToArray();

            if (res.Length > 0)
            {
                string msg = "只有状态为保存的文章才能提交，以下文章编号不符合条件：" + string.Join(",", res);
                return Json(new { isok = false,errmsg = msg });
            }

            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.AUDIT);
            return Json(new { isok = true });
        }

        [HttpPost("ReAudit")]
        public async Task<JsonResult> ReAudit(List<string> fnumbers)
        {
            var query = await formContentRepository.GetFormContentListAsync(fnumbers);
            var res = query.Where(x => !x.fdocStatus.Equals(DocStatus.AUDIT)).Select(x => x.fnumber).ToArray();

            if (res.Length > 0)
            {
                string msg = "只有审核状态下的文章才允许反审核，以下文章不符合该条件：" + string.Join(",", res);
                return Json(new { isok = false, errmsg = msg });
            }

            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.SAVE);
            return Json(new { isok = true });
        }

        [HttpPost("UnFreeze")]
        public async Task<JsonResult> UnFreeze(List<string> fnumbers)
        {
            var query = await formContentRepository.GetFormContentListAsync(fnumbers);
            var res = query.Where(x => !x.fdocStatus.Equals(DocStatus.FORBID)).Select(x => x.fnumber).ToArray();

            if (res.Length > 0)
            {
                string msg = "只有冻结状态下的文章才允许解冻，以下文章不符合该条件：" + string.Join(",", res);
                return Json(new { isok = false, errmsg = msg });
            }

            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.SAVE);
            return Json(new { isok = true });
        }

        [HttpPost("Freeze")]
        public async Task<JsonResult> Freeze(List<string> fnumbers)
        {
            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.FORBID);
            return Json(new { isok = true });
        }

        [HttpGet("DownLoadJsonFile")]
        public async Task<FileResult> DownLoadJsonFile(RaeClassContentType contentType, string fnumbers)
        {
            StringBuilder sb = new StringBuilder();
            List<string> _fnumbers = fnumbers.Split(',').ToList();
            List<FormContent> formContens= await formContentRepository.GetFormContentListAsync(_fnumbers);
            formContens.Where(x=>x.fdocStatus.Equals(DocStatus.AUDIT)).ToList().ForEach(item => {
                item.frecordFileId1 = CommonUtils.GetRecordFilePrefix(contentType) + item.frecordFileId1;
                item.frecordFileId2 = CommonUtils.GetRecordFilePrefix(contentType) + item.frecordFileId2;
                item.fcnContent = FilterHtmlStr(item.fcnContent);
                item.fenContent = FilterHtmlStr(item.fenContent);
                sb.AppendLine(JsonHelper.SerializeObject(item));
            });
            byte[] fileContents = System.Text.Encoding.Default.GetBytes(sb.ToString());
            return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, contentType +"-[" + DateTime.Now.ToString() + "].json"); //关键语句
        }

        private string FilterHtmlStr(string htmlStr)
        {
            HashSet<string> blackHtmlSet = new HashSet<string>();
            string _htmlStr = string.Empty;
            blackHtmlSet.Add("article");
            foreach (string item in blackHtmlSet)
            {
                _htmlStr = htmlStr.Replace("<"+item+">","").Replace("</"+item+">","").Replace("<" + item+"/>","");
            }
            return _htmlStr;
        }

    }
}
