using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.API.Request;
using fns.Models.API.Request.User;
using System.Security.Cryptography;
using fns.Models.API;
using fns.Utils;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();

        //GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            //register
            //DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { userName = "hahaha", password = "123456", loginUserId = "1", transId = "sdfsd" })); 
            //{d:"8GnI5XCorxsAY4vJqy6JkM6SA5K5ARyCXA8EMksTLEjbdC/x3RyGlRJNd4OJ9Z1OqF7lfqZwLxdqiPQb37l8Q0/HEBWF9igC6U9gQQTPFPM="}
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { name = "test", password = "123456", gender= "1", avatar = "", loginUserId = "1", transId = "sdfsd" }));
            return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { name = "test", password = "123456",  loginUserId = "1", transId = "sdfsd" }));
        }

        // POST api/values
        [HttpPost]
        public async Task<string> PostAsync([FromBody]RequestCommon req)
        {
            try {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        registRequest rreq = JsonConvert.DeserializeObject<registRequest>(reqStr);
                        if (!string.IsNullOrEmpty(rreq.name) && !string.IsNullOrEmpty(rreq.password))
                        {
                            if(db.User.Any(u=>u.Name == rreq.name))
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "用户名已存在！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                            var user = new Models.DB.User()
                            {
                                Name = rreq.name,
                                Password = DES_MD5Util.Encrypt(rreq.password),
                                InsDt = DateTime.Now,
                                Birthday = DateTime.Now,
                                Gender = rreq.gender,
                                Avatar = rreq.avatar,
                                Status = (int)UserStatusEnum.Normal
                            };
                            db.User.Add(user);
                            await db.SaveChangesAsync();
                            return JsonConvert.SerializeObject(new ResponseCommon("0000", "注册成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(user)), new commParameter(rreq.loginUserId, rreq.transId)));
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0002", "用户名或密码不能为空！", null, new commParameter(rreq.loginUserId, rreq.transId)));                        
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        // POST api/values
        [HttpPost("login")]
        public string Login([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        loginRequest lreq = JsonConvert.DeserializeObject<loginRequest>(reqStr);
                        var user = db.User.FirstOrDefault(u => u.Name == lreq.name);
                        if (user != null)
                        {
                            if (user.Password == DES_MD5Util.Encrypt(lreq.password))
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0000", "登录成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(user)), new commParameter(lreq.loginUserId, lreq.transId)));
                            }
                            return JsonConvert.SerializeObject(new ResponseCommon("0002", "该用户名或密码不正确！", null, new commParameter(lreq.loginUserId, lreq.transId)));
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0002", "该用户名或密码不正确！", null, new commParameter(lreq.loginUserId, lreq.transId)));
                    }
                }

                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
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
