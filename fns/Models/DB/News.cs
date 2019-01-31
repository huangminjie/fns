using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class News
    {
        public int Id { get; set; }
        public int Cid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Auth { get; set; }
        public string PicUrlList { get; set; }
        public string DoRef { get; set; }
        public int? Tag { get; set; }
        public int? UpCount { get; set; }
        public int? ViewCount { get; set; }
        public int? CommentCount { get; set; }
        public int? FocusCount { get; set; }
        public int? Status { get; set; }
        public DateTime? InsDt { get; set; }

        public virtual Category C { get; set; }
    }
}
