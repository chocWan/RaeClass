using Microsoft.AspNetCore.Mvc;
using RaeClass.Config;
using RaeClass.Helper;
using RaeClass.Models;
using RaeClass.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var res = formContentRepository.GetPageListAsync(contentType, level, titleOrContent, pageindex, pagesize);
                return Json(new { total = res.Item2, rows = res.Item1 });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("GetFormContent")]
        public JsonResult GetFormContent(string fnumber)
        {
            return Json(new { content = formContentRepository.GetFormContentAsync(fnumber) });
        }

        [HttpGet("GetFormContentList")]
        public JsonResult GetEmptyFormContentList(List<string> fnumbers)
        {
            return Json(new { content = formContentRepository.GetFormContentListAsync(fnumbers) });
        }

        [HttpGet("GetEmptyFormContent")]
        public JsonResult GetEmptyFormContent()
        {
            return Json(new { content = formContentRepository.GetEmptyFormContent() });
        }

        [HttpPost("Save")]
        public async Task<JsonResult> Save(RaeClassContentType contentType, FormContent formContent)
        {
            if (string.IsNullOrEmpty(formContent.fnumber))
            {
                FormContent _formContent = await formContentRepository.AddAsync(contentType, formContent);
                if (_formContent != null) return Json(new { IsOk = true , content = _formContent });
                else return Json(new { IsOk = false });
            }
            else {
                if (!formContent.fdocStatus.Equals(DocStatus.SAVE) || !formContent.fdocStatus.Equals(DocStatus.SUBMIT))
                {
                    throw new Exception("can not save!");
                }
                formContent.fdocStatus = DocStatus.SAVE;
                int res = await formContentRepository.UpdateAsync(formContent);
                if (res == 1) return Json(new { IsOk = true });
                else return Json(new { IsOk = false });
            }
        }

        [HttpPost("SubmitWithContent")]
        public async Task<JsonResult> Submit(RaeClassContentType contentType, List<FormContent> formContents)
        {
            formContents.ForEach(item=>item.fdocStatus = DocStatus.SUBMIT);
            int querySaveStatusCount = formContents.Where(x => string.IsNullOrEmpty(x.fnumber)).Count();
            if (querySaveStatusCount > 0) throw new Exception("there are doc being save status,please save first!");

            var querySubmit = formContents.Where(x => string.IsNullOrEmpty(x.fnumber));
            int submitCount = await formContentRepository.UpdateListAsync(formContents);
            return Json(new { IsOk = true });
        }

        [HttpPost("Submit")]
        public async Task<JsonResult> Submit(List<string> fnumbers)
        {
            var query = await formContentRepository.GetFormContentListAsync(fnumbers);
            var res = query.Where(x=>!x.fdocStatus.Equals(DocStatus.SAVE)).Select(x=>x.fnumber).ToArray();

            if (res.Length > 0)
            {
                throw new Exception("there are doc being save status or forbid status,please save first or unFreeze first !" + string.Join(",", res));
            }

            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.SUBMIT);
            return Json(new { IsOk = true });
        }

        [HttpPost("UnFreeze")]
        public async Task<JsonResult> UnFreeze(List<string> fnumbers)
        {
            var query = await formContentRepository.GetFormContentListAsync(fnumbers);
            var res = query.Where(x => !x.fdocStatus.Equals(DocStatus.FORBID)).Select(x => x.fnumber).ToArray();

            if (res.Length > 0)
            {
                throw new Exception("there are doc being not forbid status !" + string.Join(",", res));
            }

            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.SAVE);
            return Json(new { IsOk = true });
        }

        [HttpPost("Freeze")]
        public async Task<JsonResult> Freeze(List<string> fnumbers)
        {
            int submitCount = await formContentRepository.UpdateDocStatusListAsync(fnumbers, DocStatus.FORBID);
            return Json(new { IsOk = true });
        }

        [HttpGet("DownLoadJsonFile")]
        public async Task<FileResult> DownLoadJsonFile(string fnumbers)
        {
            List<string> _fnumbers = fnumbers.Split(',').ToList();
            List<FormContent> formContens= await formContentRepository.GetFormContentListAsync(_fnumbers);
            string json = JsonHelper.SerializeObject(formContens);
            byte[] fileContents = System.Text.Encoding.Default.GetBytes(json);
            return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, "read.json"); //关键语句
        }

    }
}
