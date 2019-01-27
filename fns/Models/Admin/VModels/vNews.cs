using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.Admin.VModels
{
    public class vNews
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string InsDt { get; set; }
        public string DoRef { get; set; }
    }
}
