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

        private IReadRepository readRepository;
        public RaeClassMSController(IReadRepository _readRepository)
        {
            readRepository = _readRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Read()
        {
            return View();
        }

        public IActionResult Listen()
        {
            return View();
        }

    }
}