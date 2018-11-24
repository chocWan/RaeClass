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
        public JsonResult Get(string level, string titleOrContent,int pageindex = 1,int pagesize = 10)
        {
            try
            {
                var res = readRepository.GetPageListAsync(pageindex, pagesize, level, titleOrContent);
                return Json(new { pagecount = res.Item2,reads = res.Item1 });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
            
        }

        [HttpPut]
        public async Task<JsonResult> Add(string level,string name,string cncontent,string encontent,string recordFileId1,string recordFileId2)
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

        [HttpPost]
        public async Task<JsonResult> Update(string readNumber, string level, string name, string cncontent, string encontent, string recordFileId1, string recordFileId2)
        {
            try
            {
                int res = await readRepository.UpdateAsync(readNumber,level, name, cncontent, encontent, recordFileId1, recordFileId2);
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