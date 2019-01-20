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
            this.UserName = username;
            this.Password = password;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
