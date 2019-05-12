using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.Request
{
    public class AuditRequest
    {
        public string id { get; set; }
        public bool isNormal { get; set; }
    }
}
