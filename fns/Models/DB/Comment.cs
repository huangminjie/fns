using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Comment
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public int NId { get; set; }
        public string Content { get; set; }
        public int Status { get; set; }
        public DateTime InsDt { get; set; }

        public virtual News N { get; set; }
        public virtual User U { get; set; }
    }
}
