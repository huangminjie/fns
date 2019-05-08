using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.API.Request.User;
using fns.Models.Global;
using fns.Utils;
using fns.Utils.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            #region MyRegion
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { loginUserId = "3", transId = "sdfsd", id = 29 }));
            return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { loginUserId = "3", transId = "sdfsd", ps = 15, op = 0, nids = new List<int>() { 29,39} }));
            //return DESUtil.DecryptCommonParam("zL12nTPj7Ez2GJCL9v0LHqC2tuLHLO0v5x7IGHym1aTYIXWGI8HjhBLJ72OBqtUXgOt3DXjDbBcGu4TmnrqS0dsqNiYaNW5vR00ctHy9jO8qMUG/QMpqkeHbIYLvYHwSdsyN6/wOZJGwdKHzerSusOpbbTlwYsTVKQ5btye+A5K+xnkr0NXW9hM4PoWjp+h3S72AEa+21MNOXsL/pnTt/JvKMF2a3eHgJrPl93chjJ3Xnv2eWiHzXPdwPhouGYGFSXXEjhbm5+IFIZx2YnuIfiM+Xpd9OTfzcaebtXMAsU1ZacGiNfiOeDLDzYWlpoXRF6FTZRNOiVGRKmJXbAlsO1h7+7CUr8uwe6KTTaa3DRyG5a6CmD5dSh4pQJqszL3SxDIb+PbKL0houcyUVq7YNsYWIR/S1GfW9ZjX0OJl0zOVPIkgvjaP/rXH16ggX0DC42O7wMkE1f9hFXQugMwWUg==");
            //register
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { name = "test0421", password = "123456", loginUserId = "1", transId = "sdfsd" }));
            //{d:"8GnI5XCorxsAY4vJqy6JkM6SA5K5ARyCXA8EMksTLEjbdC/x3RyGlRJNd4OJ9Z1OqF7lfqZwLxdqiPQb37l8Q0/HEBWF9igC6U9gQQTPFPM="}
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new {  loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { name = "test", password = "123456", gender= "1", avatar = "", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 9, name = "tttt1", password = "123456", avatar="sdf", birthday ="2019-5-5", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 3, name = "test1", gender = 2, password = "123456", loginUserId = "1", transId = "sdfsd" }));
            //return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { id = 3, name = "test1", gender = 2, password = "123456", loginUserId = "1", transId = "sdfsd" })); 
            #endregion
            var req = new
            {
                loginUserId = id,
                transId = "test",
                id= 39
            };
            return DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(req));
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
                            if ((await db.User.FirstOrDefaultAsync(u => u.Name == rreq.name)) != null)
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "用户名已存在！", null, new commParameter(rreq.loginUserId, rreq.transId)));

                            var categories = new List<int>();
                            await db.Category.ForEachAsync(c => {
                                categories.Add(c.Id);
                            });

                            var user = new Models.DB.User()
                            {
                                Name = rreq.name,
                                Password = DES_MD5Util.Encrypt(rreq.password),
                                InsDt = DateTime.Now,
                                Gender = 0,//rreq.gender,
                                //Avatar = rreq.avatar,
                                Status = (int)UserStatusEnum.Normal,
                                Categories = JsonConvert.SerializeObject(categories)
                        };

                            //DateTime birthday = DateTime.MinValue;
                            //var isDate = DateTime.TryParse(rreq.birthday, out birthday);
                            //if (isDate && birthday != DateTime.MinValue)
                            //    user.Birthday = birthday;

                            await db.User.AddAsync(user);
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
        public async Task<string> Login([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        loginRequest lreq = JsonConvert.DeserializeObject<loginRequest>(reqStr);
                        var user = await db.User.FirstOrDefaultAsync(u => u.Name == lreq.name);
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
        public async Task<string> Update([FromBody]RequestCommon req)
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
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == ureq.id);
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
                            if(!string.IsNullOrEmpty(ureq.avatar))
                                user.Avatar = ureq.avatar;
                            db.User.Update(user);
                            await db.SaveChangesAsync();
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

        [HttpPost("UpdateUserCategories")]
        public async Task<string> UpdateUserCategories([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        updateCategoriesRequest rreq = JsonConvert.DeserializeObject<updateCategoriesRequest>(reqStr);

                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);
                        var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                        if (user != null)
                        {
                            user.Categories = JsonConvert.SerializeObject(rreq.cIds);
                            db.User.Update(user);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", null, new commParameter(rreq.loginUserId, rreq.transId)));
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
        [HttpPost("updatepassword")]
        public async Task<string> UpdatePassword([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        passwordRequest rreq = JsonConvert.DeserializeObject<passwordRequest>(reqStr);
                        if (!string.IsNullOrEmpty(rreq.oldPassword) && !string.IsNullOrEmpty(rreq.newPassword))
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == rreq.id);
                            if (user == null)
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该用户！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                            if (DES_MD5Util.Encrypt(rreq.oldPassword) != user.Password)
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "旧密码不正确！", null, new commParameter(rreq.loginUserId, rreq.transId)));

                            user.Password = DES_MD5Util.Encrypt(rreq.newPassword);

                            await db.SaveChangesAsync();
                            return JsonConvert.SerializeObject(new ResponseCommon("0000", "密码修改成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { user = user.ToViewModel(settings.Value.ServerPath) })), new commParameter(rreq.loginUserId, rreq.transId)));
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0002", "密码不能为空！", null, new commParameter(rreq.loginUserId, rreq.transId)));
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
