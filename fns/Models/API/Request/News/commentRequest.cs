using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class commentRequest : RequestBase
    {
        public int nId { get; set; }
        public string comment { get; set; }
    }
}
