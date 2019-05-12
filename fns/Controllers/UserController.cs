using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.Admin;
using fns.Models.Global;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using fns.Models.Admin.VModels;
using fns.Utils;
using fns.Models.Admin.Request;
using Newtonsoft.Json;
using fns.Models.DB;

namespace fns.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        public UserController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        public IActionResult Lists()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<Response> GetList([FromQuery]GridPagination pager)
        {
            try
            {
                var list = new List<vUser>();
                var total = 0;
                using (fnsContext db= new fnsContext())
                {
                    total = db.User.Count();
                    await db.User.Skip((pager.pi - 1) * pager.ps).Take(pager.ps).ForEachAsync(o =>
                    {
                        list.Add(new vUser()
                        {
                            id = o.Id.ToString(),
                            name = o.Name,
                            avatar = o.Avatar,
                            gender = o.Gender.HasValue ? ((UserGenderEnum)o.Gender.Value).ToEnumText() : "",
                            status = ((UserStatusEnum)o.Status).ToEnumText(),
                            insDt = o.InsDt.ToDate(),
                            birthday = o.Birthday.ToDate()
                        });
                    });
                }
                return new Response(true, "", new GridPagination()
                {
                    pi = pager.pi,
                    ps = pager.ps,
                    total = total,
                    data = list
                });
            }
            catch (System.Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response> Delete([FromBody]DeleteRequest req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.id))
                {
                    using (fnsContext db= new fnsContext())
                    {

                        var user = await db.User.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                        db.User.Remove(user);
                        await db.SaveChangesAsync();
                    }
                    return new Response(true);
                }
                else
                {
                    return new Response(false, "请选择要删除的项");
                }
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response> SetCategory([FromBody]SetCategoryRequest req)
        {
            try
            {
                var cid = Convert.ToInt32(req.cid);
                using (fnsContext db= new fnsContext())
                {
                    await db.User.ForEachAsync((item) => {
                        var cids = item.Categories != null ? JsonConvert.DeserializeObject<List<int>>(item.Categories) : new List<int>();
                        if (!cids.Contains(cid))
                        {
                            cids.Add(cid);
                            item.Categories = JsonConvert.SerializeObject(cids);
                        }
                        db.User.Update(item);
                    });
                    await db.SaveChangesAsync();
                }
                return new Response(true);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        [HttpGet]
        public async Task<Response> GetCategoryList()
        {
            try
            {
                var list = new List<Models.Admin.VModels.vCategory>();
                using (fnsContext db= new fnsContext())
                {
                    await db.Category.ForEachAsync(o =>
                    {
                        list.Add(new vCategory()
                        {
                            id = o.Id.ToString(),
                            name = o.Name
                        });
                    });
                }
                return new Response(true, "", list);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
    }
}
