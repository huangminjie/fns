using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.Splash
{
    public class splashResponse
    {
        public int id { get; set; }
        public string picUrl { get; set; }
        public string redirectUrl { get; set; }
        public int duration { get; set; }
    }
}
