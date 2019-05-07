using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Category
{
    public class GetCategoryListRequest : RequestBase
    {
        public bool isAttentioned { get; set; }
    }
}
