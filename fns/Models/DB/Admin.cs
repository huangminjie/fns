using System;
using System.Collections.Generic;

namespace fns.Models.DB
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public DateTime? InsDt { get; set; }
        public DateTime? UpdatedDt { get; set; }
    }
}
