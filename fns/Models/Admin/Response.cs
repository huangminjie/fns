using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin
{
    public class Response
    {
        public Response(bool ok, string message = "", dynamic resData = null)
        {
            this.ok = ok;
            this.message = message;
            this.resData = resData;
        }
        public bool ok { get; set; }
        public string message { get; set; }
        public dynamic resData { get; set; }
    }
}
