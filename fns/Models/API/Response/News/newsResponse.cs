using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.DB;

namespace fns.Models.API.Response.News
{
    public class newsResponse
    {
        public int id { get; set; }
        public string title { get; set; }
        //public string content { get; set; }
        public string contentRef { get; set; }
        public string doRef { get; set; }
        public int cid { get; set; }
        public string cName { get; set; }
        public string auth { get; set; }
        public int type { get; set; }
        public List<string> picUrlList { get; set; }
        public int tag { get; set; }
        public int upCount { get; set; }
        public int viewCount { get; set; }
        public int commentCount { get; set; }
        public int focusCount { get; set; }
        public int status { get; set; }
        public string insDt { get; set; }
        public bool isCollection { get; set; }
        //public List<CommentResponse> comments { get; set; }
    }
    public class CommentResponse
    {
        public int id { get; set; }
        public User.userResponse user { get; set; }
        public int nId { get; set; }
        public int uId { get; set; }
        public string content { get; set;}
        public string insDT { get; set; }
        public int status { get; set; }
    }
}