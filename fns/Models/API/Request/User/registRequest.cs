using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.User
{
    public class registRequest: RequestBase
    {
        public string name { get; set; }
        public string password { get; set; }
        //public int gender { get; set; }
        //public string avatar { get; set; }
        //public string birthday { get; set; }
    }                 
}
