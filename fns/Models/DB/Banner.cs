using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string LinkUrl { get; set; }
        public string PicUrl { get; set; }
        public int? Type { get; set; }
        public int Cid { get; set; }

        public virtual Category C { get; set; }
    }
}
