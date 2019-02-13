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
        public IActionResult Detail(int id)
        {
            try
            {
                var item = db.News.SingleOrDefault(n => n.Id == id);
                if (item != null)
                    return View(new vNews() { id = item.Id, content = item.Content, doRef = item.DoRef, auth = item.Auth, title = item.Title, insDt = item.InsDt?.ToString("yyyy/MM/dd") });

                return Content("找不到该文章！");
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return Content("找不到该文章！");
        }

        public IActionResult Lists()
        {
            try
            {
                var pi = 1;
                var ps = 10;
                var total = db.News.Count();
                var loadData = db.News.Skip((pi - 1) * ps).Take(ps).ToList();
                return PartialView(new GridPagination() { ps = ps, total = total });
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return PartialView(null);
        }


        public IActionResult Items(int ps, int pi)
        {
            try
            {
                var list = db.News.Skip((pi - 1) * ps).Take(ps).ToList();
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
        public IActionResult EditNews(int id)
        {
            try
            {
                ViewData["ServerPath"] = settings.Value.ServerPath; // log the serverpath
                var item = db.News.SingleOrDefault(n => n.Id == id);
                if (item != null)
                    return PartialView(new vNews() { id = item.Id, content = item.Content, doRef = item.DoRef, auth = item.Auth, title = item.Title, insDt = item.InsDt?.ToString("yyyy/MM/dd") });
                return PartialView(new vNews());
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return Content("出错啦!");
        }

        public string SaveNews([FromBody]vNews req)
        {
            try
            {
                var picUrlList = string.Join("_,_", req.picUrlList.ToArray());
                db.News.Add(new News()
                {
                    Auth = "",
                    Cid = 2, // 娱乐
                    DoRef = req.doRef,
                    Content = req.content,
                    InsDt = DateTime.Now,
                    PicUrlList = picUrlList,
                    Title = req.title,
                    Status = 0
                });
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                throw;
            }
            return "ok";
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
