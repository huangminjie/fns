﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.User
{
    public class passwordRequest : RequestBase
    {
        public int id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
