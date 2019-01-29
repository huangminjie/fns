using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.Admin;
using Newtonsoft.Json;
using fns.Models.DB;
using fns.Utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace fns.Controllers
{
    public class AccountController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();
        

        public string Register(string name, string userName, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    if (db.Admin.Any(u => u.Name == userName))
                        return JsonConvert.SerializeObject(new Response(false, "用户名已存在！", null));
                    var admin = new Models.DB.Admin()
                    {
                        Name = name,
                        Password = DES_MD5Util.Encrypt(password),
                        InsDt = DateTime.Now,
                        Status = (int)AdminStatusEnum.Normal
                    };
                    db.Admin.Add(admin);
                    db.SaveChangesAsync();
                    admin.Password = null;//返回数据前清空
                    return JsonConvert.SerializeObject(new Response(true, "注册成功！", admin));
                }
                return JsonConvert.SerializeObject(new Response(false, "用户名和密码不能为空！", null));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new Response(false, ex.Message, null));
            }
        }

        public string Validate(string userName, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    var admin = db.Admin.FirstOrDefault(a => a.Name == userName);
                    //前端传来MD5加密过的密码， 后台把密码解密后MD5加密匹配
                    if (admin != null && DES_MD5Util.Md5Hash(DES_MD5Util.Decrypt(admin.Password)) == password)
                    {
                        admin.Password = null;//返回数据前清空
                        return JsonConvert.SerializeObject(new Response(true, "登录成功！", admin));
                    }
                    return JsonConvert.SerializeObject(new Response(false, "用户名或密码错误！", null));
                }
                return JsonConvert.SerializeObject(new Response(false, "用户名和密码不能为空！", null));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new Response(false, ex.Message, null));
            }
        }

        public IActionResult Login(string returnUrl = null)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<string> Login(CurrentUser user, string returnUrl = null)
        {
            if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.password))
            {
                return JsonConvert.SerializeObject(new Response(false, "用户名和密码不能为空！", null));
            }
            var lookupAdmin = db.Admin.FirstOrDefault(u => u.Name == user.userName);

            //前端传来MD5加密过的密码， 后台把密码解密后MD5加密匹配
            if (lookupAdmin == null || DES_MD5Util.Md5Hash(DES_MD5Util.Decrypt(lookupAdmin?.Password)) != user.password)
            {
                return JsonConvert.SerializeObject(new Response(false, "用户名或密码错误！", null));
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, lookupAdmin.Name));

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (returnUrl == null)
            {
                returnUrl = TempData["returnUrl"]?.ToString();
            }

            if (returnUrl != null)
            {
                return JsonConvert.SerializeObject(new Response(true, "登录成功！", returnUrl));
            }

            return JsonConvert.SerializeObject(new Response(true, "登录成功！", "/Home"));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
