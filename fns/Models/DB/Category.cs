using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Category
    {
        public Category()
        {
            Banner = new HashSet<Banner>();
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Banner> Banner { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}
