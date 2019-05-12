using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.DB;
using fns.Models.Global;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IOptions<AppSettings> settings;
        protected readonly IHostingEnvironment environment;
        public BaseController(IHostingEnvironment environment, IOptions<AppSettings> settings)
        {
            this.environment = environment;
            this.settings = settings;
        }

    }
}
