using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using fns.Models.Global;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using fns.Models.Admin;
using fns.Models.Admin.VModels;
using fns.Models.DB;
using Microsoft.EntityFrameworkCore;
using fns.Models.Admin.Request;

namespace fns.Controllers
{
    [Authorize]
    public class SplashController : BaseController
    {
        public SplashController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        public IActionResult Lists()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<Response> GetList()
        {
            try
            {
                var list = new List<Models.Admin.VModels.vSplash>();
                using (fnsContext db= new fnsContext())
                {
                    await db.Splash.ForEachAsync(o =>
                    {
                        list.Add(new vSplash()
                        {
                            id = o.Id.ToString(),
                            picUrl = o.PicUrl,
                            redirectUrl = o.RedirectUrl,
                            duration = o.Duration.HasValue ? o.Duration.Value.ToString() : ""
                        });
                    });
                }
                return new Response(true, "", list);
            }
            catch (System.Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
        [HttpPost]
        public async Task<Response> Save([FromBody]vSplash req)
        {
            try
            {
                using (fnsContext db= new fnsContext())
                {
                    if (string.IsNullOrEmpty(req.id))
                    {
                        db.Splash.Add(new Splash()
                        {
                            PicUrl = req.picUrl,
                            RedirectUrl = req.redirectUrl,
                            Duration = Convert.ToInt32(req.duration)
                        });
                    }
                    else
                    {
                        var splash = await db.Splash.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                        splash.PicUrl = req.picUrl;
                        splash.RedirectUrl = req.redirectUrl;
                        splash.Duration = Convert.ToInt32(req.duration);
                    }
                    await db.SaveChangesAsync();
                }
                return new Response(true);
            }
            catch (Exception ex)
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
                    using (fnsContext db = new fnsContext())
                    {
                        var splash = await db.Splash.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                        db.Splash.Remove(splash);
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
    }
}
