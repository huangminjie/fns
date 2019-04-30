using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class getHistoryRequest : RequestBase
    {
        public List<int> nIds { get; set; }
    }
}
