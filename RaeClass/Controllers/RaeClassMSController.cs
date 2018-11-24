using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Repository;

namespace RaeClass.Controllers
{
    public class RaeClassMSController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Read()
        {
            return View();
        }

        public IActionResult ReadDetail()
        {
            return View();
        }

        public IActionResult Listen()
        {
            return View();
        }

    }
}