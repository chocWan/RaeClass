using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Helper;
using RaeClass.Models;
using RaeClass.Repository;

namespace RaeClass.Api
{
    [Produces("application/json")]
    [Route("api/Read")]
    public class ReadController : Controller
    {

        private ILog log;
        private IReadRepository readRepository;
        public ReadController(IReadRepository _readRepository)
        {
            readRepository = _readRepository;

            log = LogManager.GetLogger(Startup.logRepository.Name, typeof(ReadController));
        }

        [HttpGet]
        public JsonResult GetReadList(string level, string titleOrContent,int pageindex = 1,int pagesize = 10)
        {
            try
            {
                var res = readRepository.GetPageListAsync(pageindex, pagesize, level, titleOrContent);
                int pagecount = res.Item2;
                var jsonDataList = res.Item1.Select(x => x.FJsonData).ToList<string>();
                var readList = JsonHelper.ConvertToModelList<Read>(jsonDataList);
                return Json(new { readList = readList });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            
        }

        public async Task<JsonResult> AddRead(string level,string name,string cncontent,string encontent,string recordFileId1,string recordFileId2)
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

        public async Task<JsonResult> UpdateRead(string readNumber, string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
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