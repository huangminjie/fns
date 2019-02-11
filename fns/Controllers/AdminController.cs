using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.Admin;
using Newtonsoft.Json;
using fns.Models.DB;
using fns.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using fns.Models.Global;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{

    [Authorize]
    public class AdminController : BaseController
    {
        public AdminController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}
