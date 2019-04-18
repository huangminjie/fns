using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Updateinfo
    {
        public int Id { get; set; }
        public int NewVer { get; set; }
        public int MinVer { get; set; }
        public string UpdateUrl { get; set; }
        public string UpdateDesc { get; set; }
        public DateTime? InsDt { get; set; }
    }
}
