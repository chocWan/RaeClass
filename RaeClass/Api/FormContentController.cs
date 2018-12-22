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

        [HttpPut("Add")]
        public async Task<JsonResult> Add(RaeClassContentType contentType, FormContent formContent)
        {
            try
            {
                int res = await formContentRepository.AddAsync(contentType,formContent);
                if (res == 1) return Json(new { IsOk = true });
                else return Json(new { IsOk = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("Update")]
        public async Task<JsonResult> Update(FormContent formContent)
        {
            try
            {
                int res = await formContentRepository.UpdateAsync(formContent);
                if (res == 1) return Json(new { IsOk = true });
                else return Json(new { IsOk = false });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("DownLoadJsonFile")]
        public async Task<FileResult> DownLoadJsonFileAsync(List<string> fnumbers)
        {
            List<FormContent> formContens= await formContentRepository.GetFormContentListAsync(fnumbers);
            string json = JsonHelper.SerializeObject(formContens);
            byte[] fileContents = System.Text.Encoding.Default.GetBytes(json);
            return File(fileContents, System.Net.Mime.MediaTypeNames.Application.Octet, "read.json"); //关键语句
        }

    }
}
