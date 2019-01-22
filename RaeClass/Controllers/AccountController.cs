using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Helper;
using RaeClass.Models;

namespace RaeClass.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = new User { FUserName = "wanchao"};
                if (user != null)
                {
                    //记录Session
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //跳转到系统首页
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "用户名或密码错误。");
                return View();
            }
            return View(model);
        }

    }
}