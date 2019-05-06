using fns.Models.API.Response;
using fns.Models.API.Response.User;
using fns.Models.API.Response.News;
using fns.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace fns.Utils.API
{
    public static class APIModelExtentions
    {
        private static fnsContext db = new fnsContext();
        public static userResponse ToViewModel(this User model, string serverPath)
        {
            userResponse vModel = new userResponse();
            vModel.id = model.Id;
            vModel.name = model.Name;
            vModel.birthday = model.Birthday.ToDate();
            vModel.gender = model.Gender ?? (int)UserGenderEnum.Unknown;
            vModel.avatar = serverPath + model.Avatar;
            vModel.status = model.Status;
            vModel.cids = model.Categories;
            return vModel;
        }
        public static newsResponse ToViewModel(this News model, string serverPath,string uid = "")
        {
            newsResponse vModel = new newsResponse();
            vModel.id = model.Id;
            vModel.title = model.Title;
            ///news/detail 网站的新闻页面
            vModel.contentRef = !string.IsNullOrEmpty(model.Content)?serverPath +"/news/detail/" +model.Id : ""; // the reference link of the custom news
            vModel.doRef = model.DoRef;
            vModel.cid = model.Cid;
            var categoryName = "";
            if (model.C == null)
            {
                categoryName = db.Category.SingleOrDefault(o => o.Id == model.Cid)?.Name;
            }
            else
                categoryName = model.C.Name;
            vModel.cName = categoryName;
            vModel.auth = model.Auth;
            vModel.type = model.Type;

            #region 判断是否被用户收藏
            vModel.isCollection = false;
            Int32.TryParse(uid, out int uId);
            var user = db.User.SingleOrDefault(u => u.Id == uId);
            if (user != null)
            {
                var collections = !string.IsNullOrEmpty(user.Collections) ? JsonConvert.DeserializeObject<List<int>>(user.Collections) : new List<int>();
                if (collections.Contains(model.Id))
                {
                    vModel.isCollection = true;
                }
            } 
            #endregion

            vModel.picUrlList = new List<string>();
            if (!string.IsNullOrEmpty(model.PicUrlList))
            {
                List<string> piclist = model.PicUrlList.Split("_,_").ToList();
                piclist.ForEach(url =>
                {
                    if (url.StartsWith("/upload/"))
                        vModel.picUrlList.Add(serverPath + url);//加上服务器地址
                    else
                        vModel.picUrlList.Add(url);
                });
            }

            vModel.tag = model.Tag ?? 0;
            vModel.upCount = model.UpCount ?? 0;
            vModel.viewCount = model.ViewCount ?? 0;
            vModel.commentCount = model.CommentCount ?? 0;
            vModel.focusCount = model.FocusCount ?? 0;
            vModel.status = model.Status ?? 0;
            vModel.insDt = model.InsDt.ToDate();
            return vModel;
        }
    }
}
