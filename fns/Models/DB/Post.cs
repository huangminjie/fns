using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Post
    {
        public Post()
        {
            Postcomment = new HashSet<Postcomment>();
        }

        public int Id { get; set; }
        public int Uid { get; set; }
        public string Content { get; set; }
        public string PicUrlList { get; set; }
        public int? UpCount { get; set; }
        public int? ViewCount { get; set; }
        public int? Status { get; set; }
        public DateTime InsDt { get; set; }
        public string DoUpList { get; set; }

        public virtual User U { get; set; }
        public virtual ICollection<Postcomment> Postcomment { get; set; }
    }
}
