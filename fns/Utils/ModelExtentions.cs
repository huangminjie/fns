using fns.Models.API.Response;
using fns.Models.API.Response.User;
using fns.Models.API.Response.News;
using fns.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Utils
{
    public static class ModelExtentions
    {
        private static FinancialNewsContext db = new FinancialNewsContext();
        public static userResponse ToViewModel(this User model)
        {
            userResponse vModel = new userResponse();
            vModel.id = model.Id;
            vModel.name = model.Name;
            vModel.birthday = model.Birthday.ToDate();
            vModel.gender = model.Gender ?? (int)UserGenderEnum.Unknown;
            vModel.avatar = model.Avatar;
            vModel.status = model.Status;
            return vModel;
        }
        public static newsResponse ToViewModel(this News model)
        {
            newsResponse vModel = new newsResponse();
            vModel.id = model.Id;
            vModel.title = model.Title;
            vModel.content = model.Content;
            vModel.doRef = model.DoRef;
            vModel.cid = model.Cid;
            var categoryName = "";
            if (model.C == null)
            {
                categoryName = db.Category.SingleOrDefault(o => o.Id == model.Cid)?.Name;
            }
            vModel.cName = model.C == null? "" : model.C.Name;
            vModel.auth = model.Auth;
            vModel.picUrlList = model.PicUrlList;
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
