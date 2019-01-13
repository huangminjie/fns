using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.API.Request.User;
using System.Security.Cryptography;
using fns.Models.Global;
using fns.Utils;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task<Response> PostAsync(registRequest req)
        {
            if (req != null)
            {
                if (!string.IsNullOrEmpty(req.userName) && !string.IsNullOrEmpty(req.passWord))
                {
                    db.User.Add(new Models.DB.User()
                    {
                        UserName = req.userName,
                        Password = MD5Util.Encrypt(req.passWord),
                        InsDt = DateTime.Now,
                        UpdatedDt = DateTime.Now,
                        Status = (int)EnumUtil.UserStatus.Normal
                    });
                    await db.SaveChangesAsync();
                    return new Response(true, "注册成功！");
                }
                return new Response(false, "用户名或密码不能为空！");
            }
            return new Response(false, "请求无效！");
        }

        // POST api/values
        [HttpPost("login")]
        public Response Login(loginRequest req)
        {
            if (req != null)
            {
                var user = db.User.FirstOrDefault(u => u.UserName == req.userName);
                if (user != null)
                {
                    if (user.Password == fns.Utils.MD5Util.Encrypt(req.passWord))
                    {
                        return new Response(true, "登录成功！");
                    }
                    return new Response(false, "该用户名或密码不正确！");
                }
                return new Response(false, "该用户名或密码不正确！");
            }
            return new Response(false,"请求无效！");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
