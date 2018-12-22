using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Config;
using RaeClass.Models;
using RaeClass.Repository;

namespace RaeClass.Controllers
{
    public class RaeClassMSController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FormContent()
        {
            string contentType = this.TempData["contentType"] as string;
            this.ViewData["contentType"] = contentType;
            return View();
        }

        /// <summary>
        /// isModify=true 修改
        /// isModify=false 新增
        /// </summary>
        /// <param name="read"></param>
        /// <param name="isModify"></param>
        /// <returns></returns>
        public IActionResult FormContentDetail(string fnumber)
        {
            if (string.IsNullOrEmpty(fnumber)) fnumber = string.Empty;
            ViewData["fnumber"] = fnumber;
            return View();
        }

        public IActionResult Read()
        {
            this.TempData["contentType"] = RaeClassContentType.Read.ToString();
            return RedirectToAction("FormContentDetail");
        }

        /// <summary>
        /// isModify=true 修改
        /// isModify=false 新增
        /// </summary>
        /// <param name="read"></param>
        /// <param name="isModify"></param>
        /// <returns></returns>
        public IActionResult ReadDetail(string fnumber)
        {
            if (string.IsNullOrEmpty(fnumber)) fnumber = string.Empty;
            ViewData["fnumber"] = fnumber;
            return View();
        }

        public IActionResult Listen()
        {
            return View();
        }

    }
}