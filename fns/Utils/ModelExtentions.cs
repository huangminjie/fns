using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.Admin.VModels;
using fns.Models.DB;

namespace fns.Utils.Admin
{
    public static class ModelExtentions
    {
        public static vNews ToViewModel(this News model)
        {
            vNews vModel = new vNews();
            vModel.id = model.Id;
            vModel.title = model.Title;
            vModel.content = model.Content;
            vModel.doRef = model.DoRef;
            vModel.cid = model.Cid;
            vModel.auth = model.Auth;
            vModel.picUrlList = model.PicUrlList.Split(',').ToList();
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
