using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class UpdateInfo
    {
        public int Id { get; set; }
        public string NewVer { get; set; }
        public string MinVer { get; set; }
        public string UpdateUrl { get; set; }
        public string UpdateDesc { get; set; }
    }
}
