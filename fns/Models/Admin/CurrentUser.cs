using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin
{
    public class CurrentUser
    {
        public CurrentUser() { }
        public CurrentUser(string username, string password)
        {
            this.userName = username;
            this.password = password;
        }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
