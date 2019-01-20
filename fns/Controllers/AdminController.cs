using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.Admin;
using Newtonsoft.Json;
using fns.Models.DB;
using fns.Utils;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    public class AdminController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
