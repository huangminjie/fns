using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Models.API.Request.News
{
    public class getCollectionRequest : RequestBase
    {
        /// <summary>
        /// id:int?; 新闻id，根据此新闻的insDt获取列表
        /// </summary>
        public int? id { get; set; }
        public int op { get; set; }
        public int ps { get; set; }
    }
}
