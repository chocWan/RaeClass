using Microsoft.AspNetCore.Mvc;
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
        public JsonResult Get(string contentType,string level, string titleOrContent, int pageindex = 1, int pagesize = 10)
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
            return Json(new { content = formContentRepository.GetBaseFormContent(fnumber) });
        }

       [HttpGet("GetEmptyFormContent")]
        public JsonResult GetEmptyFormContent()
        {
            return Json(new { content = formContentRepository.GetEmptyFormContent());
        }

        [HttpPut("Add")]
        public async Task<JsonResult> Add(string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
        {
            try
            {
                int res = await readRepository.AddAsync(level, name, cncontent, encontent, recordFileId1, recordFileId2);
                if (res == 1) return Json(new { IsOk = true });
                else return Json(new { IsOk = false });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
