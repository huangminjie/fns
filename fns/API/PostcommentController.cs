using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.API.Request.Postcomment;
using fns.Models.API.Response.Postcomment;
using fns.Models.DB;
using fns.Models.Global;
using fns.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class PostcommentController : BaseController
    {
        public PostcommentController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        [HttpPost("GetList")]
        public async Task<string> GetList([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        postcommentFilterRequest rreq = JsonConvert.DeserializeObject<postcommentFilterRequest>(reqStr);
                        DateTime dt = DateTime.Now;
                        List<Postcomment> list = new List<Postcomment>();
                        List<postcommentResponse> postList = new List<postcommentResponse>();
                        using (fnsContext db = new fnsContext())
                        {
                            Postcomment postcomment = await db.Postcomment.SingleOrDefaultAsync(o => o.Id == (rreq.id));
                            if (postcomment != null)
                            {
                                dt = postcomment.InsDt;
                            }
                            //上拉获取历史数据
                            if (rreq.op == 0)
                            {
                                list = await db.Postcomment.Where(p => p.Pid == rreq.pid && p.InsDt < dt).OrderByDescending(o => o.InsDt).Take(rreq.ps).ToListAsync();
                            }
                            //下拉获取最新数据
                            else
                            {
                                list = await db.Postcomment.Where(p => p.Pid == rreq.pid && p.InsDt > dt).OrderBy(o => o.InsDt).Take(rreq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                            }
                            postList = list.Select(p =>
                            {
                                var user = db.User.SingleOrDefault(u => u.Id == p.Uid);
                                return new postcommentResponse()
                                {
                                    id = p.Id,
                                    user = user != null ? user.Name:string.Empty,
                                    content = p.Content,
                                    replyCount = 0,
                                    status = p.Status.HasValue?p.Status.Value:0,
                                    insDt = p.InsDt.ToDate()
                                };
                            }).ToList();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { postList })), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }
        [HttpPost("Add")]
        public async Task<string> Add([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        postcommentAddRequest rreq = JsonConvert.DeserializeObject<postcommentAddRequest>(reqStr);
                        Postcomment postcomment = new Postcomment();
                        using (fnsContext db = new fnsContext())
                        {
                            postcomment.Pid = rreq.pid;
                            postcomment.Uid = Convert.ToInt32(rreq.loginUserId);
                            postcomment.Content = rreq.content;
                            postcomment.Status = Convert.ToInt32(PostCommentStatusEnum.Normal);
                            postcomment.InsDt = DateTime.Now;
                            db.Postcomment.Add(postcomment);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(postcomment)), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }
        [HttpPost("Delete")]
        public async Task<string> Delete([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        postcommentDeleteRequest rreq = JsonConvert.DeserializeObject<postcommentDeleteRequest>(reqStr);
                        using (fnsContext db = new fnsContext())
                        {
                            Postcomment postcomment = await db.Postcomment.SingleOrDefaultAsync(p=>p.Id == rreq.id);
                            postcomment.Status = Convert.ToInt32(PostCommentStatusEnum.Deleted);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(true)), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }
    }
}
