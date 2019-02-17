using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.UpdateInfo
{
    public class updateInfoResponse
    {
        public int id { get; set; }
        public int newVer { get; set; }
        public int minVer { get; set; }
        public string updateUrl { get; set; }
        public string updateDesc { get; set; }
    }
}
