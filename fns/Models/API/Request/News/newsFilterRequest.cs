using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class newsFilterRequest : RequestBase
    {
        public string title { get; set; }
        public string auth { get; set; }
    }
}
