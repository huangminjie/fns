using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Global
{
    public class Response
    {
        public Response(bool ok)
        {
            this.ok = ok;
        }
        public Response(bool ok, dynamic data) {
            this.ok = ok;
            this.data = data;
        }
        public bool ok { get; set; }
        public dynamic data { get; set; }
    }
}
