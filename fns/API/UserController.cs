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
using fns.Utils.API;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using fns.Models.Global;
using Microsoft.Extensions.Options;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        public UserController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }

        //GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { loginUserId = "1", transId = "sdfsd", cid = 2, ps = 15, op = 0, id = 0 }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { loginUserId = "1", transId = "sdfsd", id = 2 }));
            //return DESUtil.DecryptCommonParam("Kinv8nJpClfbtu2i6lajWjv7OzcJ1k0mvbR4qRli4jsw1uWvG6hZBTU5BTQ+x/cXWBXlSebQGj/i6+JdSGuqV6yor8elj9hOT4OvOnIGbI78qho+i97xguh4zZEEusGq4viCXED5rLF/cDAl1BzRZGfWXpVtLFoZxpp4tAcjp97U5CXWSaEPkraMroflYSc3mktSdQWkTMtGBhGwML1wE7QjoHd+rN7rTm2RGgTL9n0Ot1UcDncK0UbsdVVyxCxkx1Io7Ojk1lKl9GVrOcg5wNbR2fYahtfhQusKGdC+QddKW9rUjzU/12oobUYRlee1cjMu3u7bogrAZ9nnlENUYzFzAz6iUWMM");
            //register
            //DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { userName = "hahaha", password = "123456", loginUserId = "1", transId = "sdfsd" })); 
            //{d:"8GnI5XCorxsAY4vJqy6JkM6SA5K5ARyCXA8EMksTLEjbdC/x3RyGlRJNd4OJ9Z1OqF7lfqZwLxdqiPQb37l8Q0/HEBWF9igC6U9gQQTPFPM="}
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new {  loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { name = "test", password = "123456", gender= "1", avatar = "", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 9, name = "tttt1", password = "123456", avatar="sdf", birthday ="2019-5-5", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 3, name = "test1", gender = 2, password = "123456", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 3, name = "test1", gender = 2, password = "123456", loginUserId = "1", transId = "sdfsd" }));
        }

        // POST api/values
        [HttpPost]
        public async Task<string> PostAsync([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        registRequest rreq = JsonConvert.DeserializeObject<registRequest>(reqStr);
                        if (!string.IsNullOrEmpty(rreq.name) && !string.IsNullOrEmpty(rreq.password))
                        {
                            if (db.User.Any(u => u.Name == rreq.name))
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "用户名已存在！", null, new commParameter(rreq.loginUserId, rreq.transId)));

                            var user = new Models.DB.User()
                            {
                                Name = rreq.name,
                                Password = DES_MD5Util.Encrypt(rreq.password),
                                InsDt = DateTime.Now,
                                Gender = 0,//rreq.gender,
                                //Avatar = rreq.avatar,
                                Status = (int)UserStatusEnum.Normal
                            };

                            //DateTime birthday = DateTime.MinValue;
                            //var isDate = DateTime.TryParse(rreq.birthday, out birthday);
                            //if (isDate && birthday != DateTime.MinValue)
                            //    user.Birthday = birthday;

                            db.User.Add(user);
                            await db.SaveChangesAsync();
                            return JsonConvert.SerializeObject(new ResponseCommon("0000", "注册成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { user = user.ToViewModel(settings.Value.ServerPath) })), new commParameter(rreq.loginUserId, rreq.transId)));
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
                                return JsonConvert.SerializeObject(new ResponseCommon("0000", "登录成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { user = user.ToViewModel(settings.Value.ServerPath) })), new commParameter(lreq.loginUserId, lreq.transId)));
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
        [HttpPost("update")]
        public string Update([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        userRequest ureq = JsonConvert.DeserializeObject<userRequest>(reqStr);
                        if (ureq.id != 0)
                        {
                            var user = db.User.SingleOrDefault(u => u.Id == ureq.id);
                            if (user == null)
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该用户！", null, new commParameter(ureq.loginUserId, ureq.transId)));

                            //if (db.User.Any(u => u.Name == ureq.name && u.Id != ureq.id))
                            //    return JsonConvert.SerializeObject(new ResponseCommon("0002", "用户名已存在！", null, new commParameter(ureq.loginUserId, ureq.transId)));

                            //user.Name = ureq.name;
                            DateTime birthday = DateTime.MinValue;
                            var isDate = DateTime.TryParse(ureq.birthday, out birthday);
                            if (isDate && birthday != DateTime.MinValue)
                                user.Birthday = birthday;
                            user.Gender = ureq.gender;
                            user.Avatar = ureq.avatar;
                            db.User.Update(user);
                            db.SaveChanges();
                            return JsonConvert.SerializeObject(new ResponseCommon("0000", "修改成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { user = user.ToViewModel(settings.Value.ServerPath) })), new commParameter(ureq.loginUserId, ureq.transId)));
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该用户！", null, new commParameter(ureq.loginUserId, ureq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
