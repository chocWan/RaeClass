using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaeClass.Helper;
using RaeClass.Models;

namespace RaeClass.Controllers
{
    public class AccountController : Controller
    {

        public static string RETURNURL = "/RaeClassMS";

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = HttpContext.User.Claims.First().Value;
                Redirect(RETURNURL);
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Account))
                {
                    var claims = new[] { new Claim("UserName", model.Account) };
                    var claimsIdentity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal user = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user).Wait();
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsAuthenticated = false, Error = string.IsNullOrEmpty(ex.Message) ? "Invalid username or password!" : ex.Message });
            }
            return Json(new { IsAuthenticated = true, returnUrl = RETURNURL });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}