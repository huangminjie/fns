using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.Admin.VModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    public class NewsController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lists()
        {
            List<vNews> testData = new List<vNews>() {
                new vNews(){ Id = 1, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 2, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 3, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 4, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 5, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 6, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 7, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 8, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 9, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 10, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 11, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 12, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 12, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 14, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 15, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 16, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 18, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 19, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 20, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 21, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 22, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 23, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 24, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 25, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 26, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 27, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 28, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 29, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 30, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 31, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 32, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")}
            };
            var pi = 1;
            var ps = 10;
            var total = testData.Count();
            var loadData = testData.Skip((pi - 1) * ps).Take(ps).ToList();
            return PartialView(new GridPagination() { pi = pi, ps = ps, total = total, data = loadData});
        }


        public IActionResult Items(int ps, int pi)
        {
            List<vNews> testData = new List<vNews>() {
                new vNews(){ Id = 1, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 2, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 3, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 4, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 5, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 6, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 7, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 8, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 9, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 10, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 11, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 12, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 12, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 14, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 15, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 16, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 18, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 19, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 20, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 21, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 22, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 23, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 24, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 25, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 26, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 27, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 28, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 29, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 30, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 31, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")},
                new vNews(){ Id = 32, Title = "靠近建瓯六十万",Content = "看见了课件课件的司法危机", DoRef = true, NewsUrl = "", InsDt = DateTime.Now.ToString("yyyy-MM-dd")}
            };
            var a = testData.Skip((pi - 1) * ps).Take(ps).ToList();
            return PartialView(a);
        }
    }
}
