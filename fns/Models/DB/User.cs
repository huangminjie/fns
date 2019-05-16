using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class User
    {
        public User()
        {
            Comment = new HashSet<Comment>();
            Post = new HashSet<Post>();
            Postcomment = new HashSet<Postcomment>();
            Postcommentreply = new HashSet<Postcommentreply>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public int? Gender { get; set; }
        public int Status { get; set; }
        public DateTime? InsDt { get; set; }
        public DateTime? Birthday { get; set; }
        public string Collections { get; set; }
        public string Categories { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Postcomment> Postcomment { get; set; }
        public virtual ICollection<Postcommentreply> Postcommentreply { get; set; }
    }
}
