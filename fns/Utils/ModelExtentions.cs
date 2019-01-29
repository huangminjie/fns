using fns.Models.API.Response.User;
using fns.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Utils
{
    public static class ModelExtentions
    {
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
    }
}
