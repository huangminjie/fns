using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.Global;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    public class BaseController : Controller
    {
        protected readonly IOptions<AppSettings> settings;
        public BaseController(IOptions<AppSettings> settings)
        {
            this.settings = settings;
        }
    }
}
