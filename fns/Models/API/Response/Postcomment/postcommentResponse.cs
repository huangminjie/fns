using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.Postcomment
{
    public class postcommentResponse
    {
        public int id { get; set; }
        public string user { get; set; }
        public string content { get; set; }
        public int replyCount { get; set; }
        public int status { get; set; }
        public string insDt { get; set; }
    }
}
