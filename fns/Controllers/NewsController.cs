using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.Admin.VModels;
using fns.Utils.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using fns.Models.Global;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
using fns.Models.Admin;
using Microsoft.EntityFrameworkCore;
using fns.Models.Admin.Request;
using fns.Utils;

namespace fns.Controllers
{
    [Authorize]
    public class NewsController : BaseController
    {
        public NewsController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var item = await db.News.SingleOrDefaultAsync(n => n.Id == id);
                if (item != null) {
                    return PartialView(item.ToViewModel());
                }

                return Content("找不到该文章！");
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return Content("找不到该文章！");
        }

        public async Task<IActionResult> Lists(int? pi)
        {
            try
            {
                pi = pi == null ? 1 : pi;
                var ps = 10;
                var total = await db.News.CountAsync();
                var loadData = await db.News.OrderByDescending(o => o.InsDt).Skip(((pi ?? 0) - 1) * ps).Take(ps).ToListAsync();
                return PartialView(new GridPagination() { ps = ps, total = total });
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return PartialView(null);
        }


        public async Task<IActionResult> Items(int ps, int pi)
        {
            try
            {
                var list = await db.News.OrderByDescending(o => o.InsDt).Skip((pi - 1) * ps).Take(ps).ToListAsync();
                var vList = list.Select(news => news.ToViewModel()).ToList();
                return PartialView(vList);
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return PartialView(null);
        }

        //[CacheControl(HttpCacheability.NoCache), HttpGet]
        [HttpGet]
        public async Task<IActionResult> EditNews(int id)
        {
            try
            {
                ViewBag.Categories = DropdownListUntil.CategoryDropDownList();
                ViewData["ServerPath"] = settings.Value.ServerPath; // log the serverpath
                var item = await db.News.SingleOrDefaultAsync(n => n.Id == id);
                if (item != null)
                    return PartialView(item.ToViewModel());
                return PartialView(new vNews());
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return Content("出错啦!");
        }

        [HttpPost]
        public async Task<Response> SaveNews([FromBody]vNews req)
        {
            try
            {
                News model = null;
                var isAdd = true;
                if (req.id == 0)
                    model = new News();
                else
                {
                    isAdd = false;
                    model = await db.News.SingleOrDefaultAsync(n => n.Id == req.id);
                }
                model.Cid = req.cid;
                model.Title = req.title;
                model.Auth = req.auth;
                model.Content = req.content;
                model.DoRef = req.doRef;
                model.Type = req.type;
                var picUrlList = string.Join("_,_", req.picUrlList.ToArray());
                if (!string.IsNullOrEmpty(picUrlList))
                    model.PicUrlList = picUrlList;
                model.InsDt = DateTime.Now;
                model.Status = (int)NewsStatusEnum.Normal;

                if (isAdd)
                    db.News.Add(model);

                await db.SaveChangesAsync();
                return new Response(true);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message , null);
            }
        }

        [HttpPost]
        public async Task<Response> DeleteNews([FromBody]DeleteRequest req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.id))
                {
                    var news = await db.News.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                    if (news != null) {
                        db.News.Remove(news);
                        await db.SaveChangesAsync();
                        return new Response(true);
                    }
                }

                return new Response(false, "请选择要删除的新闻！");
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }


        public IActionResult CategoryLists()
        {
            return PartialView();
        }
        [HttpGet]
        public async Task<Response> GetCategoryList()
        {
            try
            {
                var list = new List<Models.Admin.VModels.vCategory>();
                await db.Category.ForEachAsync(o =>
                {
                    list.Add(new vCategory()
                    {
                        id = o.Id.ToString(),
                        name = o.Name
                    });
                });
                return new Response(true, "", list);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
        [HttpPost]
        public async Task<Response> SaveCategory([FromBody]vCategory req)
        {
            try
            {
                db.Category.Add(new Category()
                {
                    Name = req.name
                });
                await db.SaveChangesAsync();
                return new Response(true);
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
        [HttpPost]
        public async Task<Response> DeleteCategory([FromBody]DeleteRequest req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.id))
                {
                    var category = await db.Category.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                    //if (category.News.Count > 0 || category.Banner.Count > 0)
                    if (db.News.Where(o => o.Cid == category.Id).Count() > 0 || db.Banner.Where(o => o.Cid == category.Id).Count() > 0)
                        return new Response(false, "该类目正被使用，无法删除！");
                    db.Category.Remove(category);
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
