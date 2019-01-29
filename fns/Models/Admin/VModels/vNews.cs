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
    }
}
