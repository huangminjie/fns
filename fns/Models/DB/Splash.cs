using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Splash
    {
        public int Id { get; set; }
        public string PicUrl { get; set; }
        public string RedirectUrl { get; set; }
        public int? Duration { get; set; }
    }
}
