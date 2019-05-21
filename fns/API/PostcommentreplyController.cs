using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.API.Request.Postcommentreply;
using fns.Models.API.Response.Postcommentreply;
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
    public class PostcommentreplyController : BaseController
    {
        public PostcommentreplyController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
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
                        postcommentreplyFilterRequest rreq = JsonConvert.DeserializeObject<postcommentreplyFilterRequest>(reqStr);
                        DateTime dt = DateTime.Now;
                        List<Postcommentreply> list = new List<Postcommentreply>();
                        List<postcommentreplyResponse> replyList = new List<postcommentreplyResponse>();
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
                            Postcommentreply postcommentreply = await db.Postcommentreply.SingleOrDefaultAsync(o => o.Id == (rreq.id));
                            if (postcommentreply != null)
                            {
                                dt = postcommentreply.InsDt;
                            }
                            //上拉获取历史数据
                            if (rreq.op == 0)
                            {
                                list = await db.Postcommentreply.Where(p => p.Pcid == rreq.pcid && p.InsDt < dt).OrderByDescending(o => o.InsDt).Take(rreq.ps).ToListAsync();
                            }
                            //下拉获取最新数据
                            else
                            {
                                list = await db.Postcommentreply.Where(p => p.Pcid == rreq.pcid && p.InsDt > dt).OrderBy(o => o.InsDt).Take(rreq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                            }
                            var uIds = db.Postcommentreply.Select(pcr => pcr.Uid).ToList();
                            var users = db.User.Where(u => uIds.Contains(u.Id));
                            replyList = list.Select(p =>
                            {
                                var user = users.SingleOrDefault(u => u.Id == p.Uid);
                                var doUpList = string.IsNullOrEmpty(p.DoUpList) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(p.DoUpList);
                                return new postcommentreplyResponse()
                                {
                                    id = p.Id,
                                    user = user != null ?  user.ToViewModel(settings.Value.ServerPath): null,
                                    content = p.Content,
                                    upCount = p.UpCount ?? 0,
                                    status = p.Status.HasValue ? p.Status.Value : 0,
                                    insDt = p.InsDt.ToDate(),
                                    doUp = user == null ? false : doUpList.Contains(user.Id)
                                };
                            }).ToList();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { replyList = replyList })), new commParameter(rreq.loginUserId, rreq.transId)));
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
                        postcommentreplyAddRequest rreq = JsonConvert.DeserializeObject<postcommentreplyAddRequest>(reqStr);
                        Postcommentreply postcommentreply = new Postcommentreply();
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);

                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "请先登录！", null, new commParameter(rreq.loginUserId, rreq.transId)));
                            }
                            postcommentreply.Pcid = rreq.pcid;
                            postcommentreply.Uid = Convert.ToInt32(rreq.loginUserId);
                            postcommentreply.Content = rreq.content;
                            postcommentreply.Status = Convert.ToInt32(PostCommentReplyStatusEnum.Normal);
                            postcommentreply.InsDt = DateTime.Now;
                            db.Postcommentreply.Add(postcommentreply);
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
                            Postcommentreply postcommentreply = await db.Postcommentreply.SingleOrDefaultAsync(p => p.Id == rreq.id);
                            if (postcommentreply != null)
                            {
                                postcommentreply.Status = Convert.ToInt32(PostCommentReplyStatusEnum.Deleted);
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
                        postcommentreplyResponse vPost = new postcommentreplyResponse();
                        var uId = 0;
                        Int32.TryParse(preq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                uId = 0;
                            }
                            var postCommentReply = await db.Postcommentreply.SingleOrDefaultAsync(pc => pc.Id == preq.id);
                            if (postCommentReply == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该回复！", null, new commParameter(preq.loginUserId, preq.transId)));
                            }
                            var doUpList = string.IsNullOrEmpty(postCommentReply.DoUpList) ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(postCommentReply.DoUpList);
                            if (doUpList.Contains(uId))
                            {
                                //取消点赞
                                doUpList.Remove(uId);
                            }
                            else
                            {
                                //点赞
                                doUpList.Add(uId);
                                postCommentReply.UpCount = (postCommentReply.UpCount ?? 0) + 1;
                            }

                            postCommentReply.DoUpList = JsonConvert.SerializeObject(doUpList);
                            db.Postcommentreply.Update(postCommentReply);
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
