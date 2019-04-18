using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.Banner
{
    public class bannerResponse
    {
        public int cid { get; set; }
        public string linkUrl{ get; set; }
        public string picUrl { get; set; }
        public int type { get; set; }
    }
}
