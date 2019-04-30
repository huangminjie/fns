using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.User
{
    public class updateCategoriesRequest : RequestBase
    {
        public List<int> cIds { get; set; }
    }
}
