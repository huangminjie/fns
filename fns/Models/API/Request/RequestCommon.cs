using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request
{
    public class RequestCommon
    {
        public string d { get; set; }
    }
    public class RequestBase
    {
        public string loginUserId { get; set; }
        public string transId { get; set; }
    }
}
