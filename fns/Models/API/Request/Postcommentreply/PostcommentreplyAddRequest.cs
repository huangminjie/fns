using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Postcommentreply
{
    public class postcommentreplyAddRequest : RequestBase
    {
        public int pcid { get; set; }
        public string content { get; set; }
    }
}
