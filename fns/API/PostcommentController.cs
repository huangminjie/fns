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
using fns.Utils.API;
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
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            if (rreq.isMine)
                            {
                                var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                                if (user == null)
                                {
                                    return JsonConvert.SerializeObject(new ResponseCommon("0002", "请先登录！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                                }
                            }
                            Postcomment postcomment = await db.Postcomment.SingleOrDefaultAsync(o => o.Id == (rreq.id));
                            if (postcomment != null)
                            {
                                dt = postcomment.InsDt;
                            }
                            //上拉获取历史数据
                            if (rreq.op == 0)
                            {
                                list = await db.Postcomment.Where(p => p.Pid == rreq.pid && (rreq.isMine ? p.Uid == uId : true) && p.InsDt < dt).OrderByDescending(o => o.InsDt).Take(rreq.ps).ToListAsync();
                            }
                            //下拉获取最新数据
                            else
                            {
                                list = await db.Postcomment.Where(p => p.Pid == rreq.pid && (rreq.isMine ? p.Uid == uId : true) && p.InsDt > dt).OrderBy(o => o.InsDt).Take(rreq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                            }
                            var pcIds = db.Postcomment.Select(pc=>pc.Id).ToList();
                            var replys = db.Postcommentreply.Where(pcr=>pcIds.Contains(pcr.Pcid));
                            var uIds = db.Postcomment.Select(pc => pc.Uid).ToList();
                            var users = db.User.Where(u=> uIds.Contains(u.Id));
                            postList = list.Select(p =>
                            {
                                var user = users.SingleOrDefault(u => u.Id == p.Uid);
                                var doUpList = string.IsNullOrEmpty(p.DoUpList) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(p.DoUpList);
                                return new postcommentResponse()
                                {
                                    id = p.Id,
                                    user = user != null ? user.ToViewModel(settings.Value.ServerPath) : null,
                                    content = p.Content,
                                    replyCount = replys.Where(r => r.Pcid == p.Id).Count(),
                                    upCount = p.UpCount ?? 0,
                                    status = p.Status.HasValue ? p.Status.Value : 0,
                                    insDt = p.InsDt.ToDate(),
                                    doUp = user == null ? false : doUpList.Contains(user.Id)
                                };
                            }).ToList();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { postList = postList })), new commParameter(rreq.loginUserId, rreq.transId)));
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
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);

                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "请先登录！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                            }
                            postcomment.Pid = rreq.pid;
                            postcomment.Uid = uId;
                            postcomment.Content = rreq.content;
                            postcomment.Status = Convert.ToInt32(PostCommentStatusEnum.Normal);
                            postcomment.InsDt = DateTime.Now;
                            db.Postcomment.Add(postcomment);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", null, new commParameter(rreq.loginUserId, rreq.transId)));
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
                        idRequest rreq = JsonConvert.DeserializeObject<idRequest>(reqStr);
                        using (fnsContext db = new fnsContext())
                        {
                            Postcomment postcomment = await db.Postcomment.SingleOrDefaultAsync(p=>p.Id == rreq.id);
                            if (postcomment != null)
                            {
                                postcomment.Status = Convert.ToInt32(PostCommentStatusEnum.Deleted);
                                await db.SaveChangesAsync();
                            }
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }


        [HttpPost("Up")]
        public async Task<string> Up([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        idRequest preq = JsonConvert.DeserializeObject<idRequest>(reqStr);
                        postcommentResponse vPost = new postcommentResponse();
                        var uId = 0;
                        Int32.TryParse(preq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                uId = 0;
                            }
                            var postComment = await db.Postcomment.SingleOrDefaultAsync(pc => pc.Id == preq.id);
                            if (postComment == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该评论！", null, new commParameter(preq.loginUserId, preq.transId)));
                            }
                            var doUpList = string.IsNullOrEmpty(postComment.DoUpList) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(postComment.DoUpList);
                            if (doUpList.Contains(uId))
                            {
                                //取消点赞
                                doUpList.Remove(uId);
                            }
                            else
                            {
                                //点赞
                                doUpList.Add(uId);
                                postComment.UpCount = (postComment.UpCount ?? 0) + 1;
                            }

                            postComment.DoUpList = JsonConvert.SerializeObject(doUpList);
                            db.Postcomment.Update(postComment);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", null, new commParameter(preq.loginUserId, preq.transId)));

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
