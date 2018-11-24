﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaeClass.Api
{
    [Produces("application/json")]
    [Route("api/Demo")]
    public class DemoController : Controller
    {
        // GET: api/Demo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Demo/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public JsonResult GetDemo(int id)
        {
            return Json(new { name = "value"+id });
        }

        // POST: api/Demo
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Demo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}