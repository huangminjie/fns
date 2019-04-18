using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using fns.Models.Global;
using fns.Models.Admin;
using Microsoft.EntityFrameworkCore;
using fns.Models.Admin.VModels;
using fns.Models.DB;
using fns.Models.Admin.Request;
using fns.Utils;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    [Authorize]
    public class BannerController : BaseController
    {
        public BannerController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        // GET: /<controller>/
        public IActionResult Lists()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<Response> GetList()
        {
            try
            {
                var categories = DropdownListUntil.CategoryDropDownList().ToList();
                var list = new List<vBanner>();
                await db.Banner.ForEachAsync(o =>
                {
                    string categoryName = "";
                    if (o.C != null)
                        categoryName = o.C.Name;
                    else
                    {
                        var category = db.Category.SingleOrDefaultAsync(c => c.Id == o.Cid);
                        if (category != null)
                            categoryName = category.Result.Name;
                    }
                    list.Add(new vBanner()
                    {
                        id = o.Id.ToString(),
                        picUrl = o.PicUrl,
                        linkUrl = o.LinkUrl,
                        type = o.Type.HasValue ? o.Type.Value.ToString() : "",
                        cid = o.Cid.ToString(),
                        cName = categoryName
                    });
                });
                return new Response(true, "", new { categories = categories, list = list });
            }
            catch (System.Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
        [HttpPost]
        public async Task<Response> Save([FromBody]vBanner req)
        {
            try
            {
                if (string.IsNullOrEmpty(req.id))
                {
                    db.Banner.Add(new Banner()
                    {
                        PicUrl = req.picUrl,
                        LinkUrl = req.linkUrl,
                        Type = Convert.ToInt32(req.type),
                        Cid = Convert.ToInt32(req.cid)
                    });
                }
                else
                {
                    var banner = await db.Banner.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                    banner.PicUrl = req.picUrl;
                    banner.LinkUrl = req.linkUrl;
                    banner.Type = Convert.ToInt32(req.type);
                    banner.Cid = Convert.ToInt32(req.cid);
                }
                await db.SaveChangesAsync();
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
                    var banner = await db.Banner.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                    db.Banner.Remove(banner);
                    await db.SaveChangesAsync();
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
