using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Postcomment
{
    public class postcommentFilterRequest: RequestBase
    {
        public int pid { get; set; }
        public int id { get; set; }
        public int op { get; set; }
        public int ps { get; set; }
    }
}
