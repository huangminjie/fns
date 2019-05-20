using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Postcomment
{
    public class postcommentDeleteRequest : RequestBase
    {
        public int id { get; set; }
    }
}
