using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class newsFilterRequest : RequestBase
    {
        public int? id { get; set; }
        /// <summary>
        /// 上拉=0，下拉=1
        /// </summary>
        public int op { get; set; }
        public string title { get; set; }
        public string auth { get; set; }
        public int cid { get; set; }
        public int ps { get; set; }
    }
}
