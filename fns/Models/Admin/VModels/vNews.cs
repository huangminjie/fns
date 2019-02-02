using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.VModels
{
    public class vNews
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string insDt { get; set; }
        public string doRef { get; set; }
        public int cid { get; set; }
        public string auth { get; set; }
        public List<string> picUrlList { get; set; }
        public int tag { get; set; }
        public int upCount { get; set; }
        public int viewCount { get; set; }
        public int commentCount { get; set; }
        public int focusCount { get; set; }
        public int status { get; set; }
    }
}
