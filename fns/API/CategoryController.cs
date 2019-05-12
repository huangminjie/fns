using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.DB;
using fns.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using fns.Models.API.Response.Category;
using Microsoft.EntityFrameworkCore;
using fns.Models.API.Request.Category;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        [HttpPost("GetCategories")]
        public async Task<string> GetCategories([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        GetCategoryListRequest rreq = JsonConvert.DeserializeObject<GetCategoryListRequest>(reqStr);
                        List<categoryResponse> list = new List<categoryResponse>();
                        var res = new List<categoryResponse>();
                        using (fnsContext db = new fnsContext())
                        {
                            var categories = await db.Category.ToListAsync();
                            categories.ForEach(c => {
                                list.Add(new categoryResponse()
                                {
                                    id = c.Id,
                                    name = c.Name
                                });
                            });
                            Int32.TryParse(rreq.loginUserId, out int uId);
                            var user = db.User.SingleOrDefault(u => u.Id == uId);
                            if (user != null)
                            {
                                var user_categories = !string.IsNullOrEmpty(user.Categories) ? JsonConvert.DeserializeObject<List<int>>(user.Categories) : new List<int>();
                                if (rreq.isAttentioned)
                                {
                                    foreach (var item in user_categories)
                                    {
                                        res.Add(list.SingleOrDefault(o => o.id == item));
                                    }
                                }
                                else
                                {
                                    res = list.Where(o => !user_categories.Contains(o.id)).ToList();
                                }
                            }
                            else
                            {
                                if (!rreq.isAttentioned)
                                {
                                    res = new List<categoryResponse>();
                                }
                                else
                                {
                                    res = list;
                                }
                            }
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { categories = res })), new commParameter(rreq.loginUserId, rreq.transId)));

                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }
    }
}
