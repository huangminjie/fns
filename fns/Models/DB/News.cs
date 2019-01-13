using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? InsDt { get; set; }
        public bool DoRef { get; set; }
        public string NewsUrl { get; set; }
    }
}
