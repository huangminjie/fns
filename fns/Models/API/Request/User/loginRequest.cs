using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.User
{
    public class loginRequest: RequestBase
    {
        public string name { get; set; }
        public string password { get; set; }
    }
}
