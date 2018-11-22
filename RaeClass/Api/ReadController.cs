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

        //private ILog log;
        private IReadRepository readRepository;
        public ReadController(IReadRepository _readRepository)
        {
            readRepository = _readRepository;
            //log = LogManager.GetLogger(Startup.logRepository.Name, typeof(ReadController));
        }

        [HttpGet]
        public JsonResult GetReadList(string level, string titleOrContent,int pageindex = 1,int pagesize = 10)
        {
            var res = readRepository.GetPageListAsync(pageindex,pagesize,level,titleOrContent);
            int pagecount = res.Item2;
            var jsonDataList = res.Item1.Select(x=>x.FJsonData).ToList<string>();
            var readList = JsonHelper.ConvertToModelList<Read>(jsonDataList);

            return Json(new { readList = readList });
        }

    }
}