using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.VModels
{
    public class GridPagination
    {
        public int pi { get; set; }
        public int ps { get; set; }
        public int total { get; set; }
        public dynamic data { get; set; }
    }
}
