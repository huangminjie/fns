using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public int? Gender { get; set; }
        public int Status { get; set; }
        public DateTime? InsDt { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
