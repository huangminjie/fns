using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.Admin.VModels;
using fns.Utils;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    public class NewsController : Controller
    {
        private IHostingEnvironment environment { get; set; }
        public NewsController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        List<vNews> testData = new List<vNews>() {
                new vNews(){ id = 1, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 2, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 3, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 4, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 5, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 6, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 7, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 8, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 9, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 10, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 11, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 12, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 12, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 14, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 15, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 16, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 18, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 19, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 20, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 21, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 22, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 23, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 24, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 25, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 26, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 27, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 28, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 29, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 30, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 31, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ id = 32, title = "靠近建瓯六十万",content = "看见了课件课件的司法危机", doRef = "", insDt = DateTime.Now.ToString("yyyy-MM-dd")}
            };
        private FinancialNewsContext db = new FinancialNewsContext();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lists()
        {
            try
            {
                var pi = 1;
                var ps = 10;
                var total = testData.Count();
                var loadData = testData.Skip((pi - 1) * ps).Take(ps).ToList();
                return PartialView(new GridPagination() { ps = ps, total = total});
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
                var a = testData.Skip((pi - 1) * ps).Take(ps).ToList();
                return PartialView(a);
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
                //var item = testData.FirstOrDefault(n => n.Id == id);
                var item = db.News.OrderByDescending(o=>o.Id).FirstOrDefault();

                var root = environment.WebRootPath;
                var fullPath = $@"{root}\uploadnewshtml\{item.Content}";
                item.Content=FileUntil.ReadFromHTML(fullPath);
                return PartialView(new vNews() { id = item.Id, content = item.Content, doRef = item.DoRef, title = item.Title, insDt = item.InsDt?.ToString("yyyy/MM/dd") });
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            return PartialView(null);
        }

        public string SaveNews([FromBody]vNews req)
        {
            try
            {
                var s = req.title;

                var root = environment.WebRootPath;
                var filename = "news_" + System.DateTime.Now.ToString("yyMMddHHmmssfff") + ".html";
                var fullPath = $@"{root}\uploadnewshtml\{filename}";
                FileUntil.SaveIntoHTML(fullPath, req.content);
                

                db.News.Add(new News()
                {
                    Auth = "",
                    Cid = 2, // 娱乐
                    DoRef = req.doRef,
                    Content = filename,
                    InsDt = DateTime.Now,
                    PicUrlList = "",
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
    }
}
