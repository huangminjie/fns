using fns.Models.API.Response;
using fns.Models.API.Response.User;
using fns.Models.API.Response.News;
using fns.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using fns.Models.API.Response.Post;
using fns.Models.API.Response.Postcomment;

namespace fns.Utils.API
{
    public static class APIModelExtentions
    {
        //private static fnsContext db = new fnsContext();
        public static userResponse ToViewModel(this User model, string serverPath)
        {
            userResponse vModel = new userResponse();
            vModel.id = model.Id;
            vModel.name = model.Name;
            vModel.birthday = model.Birthday.ToDate();
            vModel.gender = model.Gender ?? (int)UserGenderEnum.Unknown;
            vModel.avatar = string.IsNullOrEmpty(model.Avatar)? serverPath + "/img/avatar.jpg": serverPath + model.Avatar;
            vModel.status = model.Status;
            vModel.cids = model.Categories;
            return vModel;
        }
        public static newsResponse ToViewModel(this News model, string serverPath)
        {
            newsResponse vModel = new newsResponse();
            vModel.id = model.Id;
            vModel.title = model.Title;
            ///news/detail 网站的新闻页面
            vModel.contentRef = !string.IsNullOrEmpty(model.Content) ? serverPath + "/news/detail/" + model.Id : ""; // the reference link of the custom news
            vModel.doRef = model.DoRef;
            vModel.cid = model.Cid;
            if (model.C != null)
            {
                vModel.cName = model.C.Name;
            }
            vModel.auth = model.Auth;
            vModel.type = model.Type;

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
        public static CommentResponse ToViewModel(this Comment model)
        {
            CommentResponse vModel = new CommentResponse() {
                id = model.Id,
                nId = model.NId,
                uId = model.UId,
                content = model.Content,
                status = model.Status,
                insDT = model.InsDt.ToDate()
            };
            return vModel;
        }


        public static postResponse ToViewModel(this Post model, int loginUserId, Models.DB.User user, string serverPath)
        {
            postResponse vModel = new postResponse();
            vModel.id = model.Id;
            vModel.user = user == null ? null : user.ToViewModel(serverPath);
            vModel.content = model.Content;
            vModel.upCount = model.UpCount ?? 0;
            vModel.viewCount = model.ViewCount ?? 0;
            vModel.status = model.Status ?? (int)PostStatusEnum.Normal;
            var doUpList = string.IsNullOrEmpty(model.DoUpList) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(model.DoUpList);
            vModel.doUp = loginUserId == 0 ? false : (doUpList.Contains(loginUserId));
            vModel.picUrlList = new List<string>();
            if (!string.IsNullOrEmpty(model.PicUrlList))
            {
                List<string> piclist = JsonConvert.DeserializeObject<List<string>>(model.PicUrlList);
                piclist.ForEach(url =>
                {
                    vModel.picUrlList.Add(serverPath + url);//加上服务器地址
                });
            }
            vModel.insDt = model.InsDt.ToDate();
            return vModel;
        }
        
    }
}
