using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.VModels
{
    public class vUpdateInfo
    {
        public int id { get; set; }
        public string newVer { get; set; }
        public string minVer { get; set; }
        public string updateUrl { get; set; }
        public string updateDesc { get; set; }
    }
}
