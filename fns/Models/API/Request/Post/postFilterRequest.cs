using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.Post
{
    public class postFilterRequest : RequestBase
    {
        /// <summary>
        /// id:int?; postId，根据此帖的insDt分页获取列表
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// isMine; 是否获取我的发帖
        /// </summary>
        public bool isMine { get; set; }
        /// <summary>
        /// 上拉=0，下拉=1
        /// </summary>
        public int op { get; set; }
        public int ps { get; set; }
    }
}
