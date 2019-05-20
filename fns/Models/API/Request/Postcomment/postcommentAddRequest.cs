using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Postcomment
{
    public class postcommentAddRequest : RequestBase
    {
        public int pid { get; set; }
        public string content { get; set; }
    }
}
