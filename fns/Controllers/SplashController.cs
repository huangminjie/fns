using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    [Authorize]
    public class SplashController : Controller
    {
        public IActionResult Lists()
        {
            var list = new List<Models.Admin.VModels.vSplash>();
            list.Add(new Models.Admin.VModels.vSplash() 
            {
                id = "1",
                redirectUrl = "www.baidu.com",
                picUrl = "https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=2230167403,4188800858&fm=27&gp=0.jpg",
                duration = 3
            });
            return PartialView(list);
        }
    }
}
