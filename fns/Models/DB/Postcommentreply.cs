using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Postcommentreply
    {
        public int Id { get; set; }
        public int Pcid { get; set; }
        public int Uid { get; set; }
        public string Content { get; set; }
        public int? UpCount { get; set; }
        public int? Status { get; set; }
        public DateTime InsDt { get; set; }

        public virtual Postcomment Pc { get; set; }
        public virtual User U { get; set; }
    }
}
