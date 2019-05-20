using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Postcomment
    {
        public Postcomment()
        {
            Postcommentreply = new HashSet<Postcommentreply>();
        }

        public int Id { get; set; }
        public int Pid { get; set; }
        public int Uid { get; set; }
        public string Content { get; set; }
        public int? ReplyCount { get; set; }
        public int? Status { get; set; }
        public DateTime InsDt { get; set; }

        public virtual Post P { get; set; }
        public virtual User U { get; set; }
        public virtual ICollection<Postcommentreply> Postcommentreply { get; set; }
    }
}
