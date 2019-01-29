using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Response.User
{
    public class userResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string avatar { get; set; }
        public int gender { get; set; }
        public int status { get; set; }
        public string birthday { get; set; }
    }
}
