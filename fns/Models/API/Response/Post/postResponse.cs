using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.Post
{
    public class postResponse
    {
        public int id { get; set; }
        public Response.User.userResponse user { get; set; }
        public string content { get; set; }
        public List<string> picUrlList { get; set; }
        public int upCount { get; set; }
        public int viewCount { get; set; }
        public int commentCount { get; set; }
        public int status { get; set; }
        public string insDt { get; set; }
        public bool doUp { get; set; }
    }
}
