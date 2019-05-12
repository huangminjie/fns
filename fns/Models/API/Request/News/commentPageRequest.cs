using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class commentPageRequest : RequestBase
    {
        /// <summary>
        /// id:int?; 评论id，根据此评论的insDt分页获取列表
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// 上拉=0，下拉=1
        /// </summary>
        public int op { get; set; }
        public int nid { get; set; }
        public int ps { get; set; }
    }
}
