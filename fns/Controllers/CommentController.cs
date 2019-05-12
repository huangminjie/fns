using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.Admin;
using fns.Models.Admin.Request;
using fns.Models.Admin.VModels;
using fns.Models.DB;
using fns.Models.Global;
using fns.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace fns.Controllers
{
    public class CommentController : BaseController
    {
        public CommentController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        public IActionResult Lists()
        {
            return PartialView();
        }

        [HttpGet]
        public async Task<Response> GetList([FromQuery]GridPagination pager)
        {
            try
            {
                var list = new List<vComment>();
                var total = 0;
                using (fnsContext db = new fnsContext())
                {
                    total = db.Comment.Count();
                    await db.Comment.Skip((pager.pi - 1) * pager.ps).Take(pager.ps).ForEachAsync(async o =>
                    {
                        var user = await db.User.SingleOrDefaultAsync(u=>u.Id == o.UId);
                        var news = await db.News.SingleOrDefaultAsync(n=>n.Id == o.NId);
                        list.Add(new vComment()
                        {
                            id = o.Id.ToString(),
                            user = user.Name,
                            news = news.Title,
                            content = o.Content,
                            status = o.Status == 0 ? "正常" : "违规",
                            insDt = o.InsDt.ToDate()
                        });
                    });
                }
                return new Response(true, "", new GridPagination()
                {
                    pi = pager.pi,
                    ps = pager.ps,
                    total = total,
                    data = list
                });
            }
            catch (System.Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response> Audit([FromBody]AuditRequest req)
        {
            try
            {
                if (!string.IsNullOrEmpty(req.id))
                {
                    using (fnsContext db = new fnsContext())
                    {

                        var comment = await db.Comment.SingleOrDefaultAsync(o => o.Id == Convert.ToInt32(req.id));
                        comment.Status = req.isNormal ? 0 : 1;
                        await db.SaveChangesAsync();
                    }
                    return new Response(true);
                }
                else
                {
                    return new Response(false, "请选择要删除的项");
                }
            }
            catch (Exception ex)
            {
                return new Response(false, ex.Message);
            }
        }
    }
}