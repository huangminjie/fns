using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API
{
    public class ResponseCommon
    {
        public ResponseCommon(string code, string desc, dynamic retData, commParameter commParam) {
            this.code = code;
            this.desc = desc;
            this.retData = retData;
            this.commParam = commParam;
        }
        public string code { get; set; }
        public string desc { get; set; }
        public dynamic retData { get; set; }
        public commParameter commParam { get; set; }
    }
    public class commParameter
    {
        public commParameter(string loginUserId, string transId)
        {
            this.loginUserId = loginUserId;
            this.transId = transId;
        }
        public string loginUserId { get; set; }
        public string transId { get; set; }
    }
}
