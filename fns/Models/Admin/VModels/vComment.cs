using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.VModels
{
    public class vComment
    {
        public string id { get; set; }
        public string user { get; set; }
        public string news { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string insDt { get; set; }
    }
}
