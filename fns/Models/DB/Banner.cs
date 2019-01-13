using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Banner
    {
        public int Id { get; set; }
        public string LinkUrl { get; set; }
        public string PicUrl { get; set; }
    }
}
