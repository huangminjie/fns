using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Post
{
    public class addRequest : RequestBase
    {
        public string content { get; set; }
        public string picUrlList { get; set; }
    }
}
