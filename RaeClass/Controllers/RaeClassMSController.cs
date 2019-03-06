using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Config;
using RaeClass.Models;
using RaeClass.Repository;

namespace RaeClass.Controllers
{
    [Authorize]
    public class RaeClassMSController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FormContent(string contentType)
        {
            ViewData["contentType"] = contentType;
            return View();
        }

        /// <summary>
        /// isModify=true 修改
        /// isModify=false 新增
        /// </summary>
        /// <param name="read"></param>
        /// <param name="isModify"></param>
        /// <returns></returns>
        public IActionResult FormContentDetail(RaeClassContentType contentType,string fnumber)
        {
            if (string.IsNullOrEmpty(fnumber)) fnumber = string.Empty;
            ViewData["fnumber"] = fnumber;
            ViewData["contentType"] = contentType;
            return View();
        }

        public IActionResult FormContentSearch(string queryStr)
        {
            if (string.IsNullOrEmpty(queryStr)) queryStr = string.Empty;
            ViewData["queryStr"] = queryStr;
            return View();
        }

    }
}