using fns.Models.DB;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fns.Utils
{
    public static class DropdownListUntil
    {
        #region Category

        public static IEnumerable<SelectListItem> CategoryDropDownList()
        {
            var result = new List<SelectListItem> { new SelectListItem
            {
                Selected = false,
                Text = "--类目--",
                Value =  string.Empty
            }};

            
            using (var db = new fnsContext())
            {
                var Categories = db.Category;
                result.AddRange(Categories.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id.ToString()
                }));
            }
            return result;
        }

        #endregion
    }
}
