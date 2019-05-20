using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.API.Request.Post;
using fns.Models.API.Response.Post;
using fns.Models.DB;
using fns.Models.Global;
using fns.Utils;
using fns.Utils.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class PostController : BaseController
    {
        public PostController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }


        [HttpPost("GetById")]
        public async Task<string> GetById([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        idRequest preq = JsonConvert.DeserializeObject<idRequest>(reqStr);
                        postResponse vPost = new postResponse();
                        var uId = 0;
                        Int32.TryParse(preq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                uId = 0;
                            }
                            var post = await db.Post.SingleOrDefaultAsync(p => p.Id == preq.id);
                            if (post == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该帖子！", null, new commParameter(preq.loginUserId, preq.transId)));
                            }
                            post.ViewCount = (post.ViewCount ?? 0) + 1;
                            
                            db.Post.Update(post);
                            await db.SaveChangesAsync();
                            vPost = post.ToViewModel(uId, settings.Value.ServerPath);
                            vPost.commentCount = db.Postcomment.Where(p => p.Pid == vPost.id).Count();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { post = vPost })), new commParameter(preq.loginUserId, preq.transId)));

                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
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
                        postFilterRequest preq = JsonConvert.DeserializeObject<postFilterRequest>(reqStr);
                        DateTime dt = DateTime.Now;
                        List<Post> list = new List<Post>();
                        List<postResponse> postList = new List<postResponse>();
                        var uId = 0;
                        Int32.TryParse(preq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            if (preq.isMine)
                            {
                                var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                                if (user == null)
                                {
                                    return JsonConvert.SerializeObject(new ResponseCommon("0002", "请先登录！", null, new commParameter(preq.loginUserId, preq.transId)));
                                }
                            }
                            Post post = await db.Post.SingleOrDefaultAsync(o => o.Id == (preq.id ?? 0));
                            if (post != null)
                            {
                                dt = post.InsDt;
                            }
                            //上拉获取历史数据
                            if (preq.op == 0)
                            {
                                list = await db.Post.Where(p => p.Status == (int)PostStatusEnum.Normal && (preq.isMine ? p.Uid == uId : true) && p.InsDt < dt).OrderByDescending(o => o.InsDt).Take(preq.ps).ToListAsync();
                            }
                            //下拉获取最新数据
                            else
                            {
                                list = await db.Post.Where(p => p.Status == (int)PostStatusEnum.Normal && (preq.isMine ? p.Uid == uId : true) && p.InsDt > dt).OrderBy(o => o.InsDt).Take(preq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                            }
                            var pIds = list.Select(p=>p.Id).ToList();
                            var pComments = db.Postcomment.Where(pc => pIds.Contains(pc.Pid));
                            list.ForEach(p=> {
                                var vPost = p.ToViewModel(uId, settings.Value.ServerPath);
                                vPost.commentCount = pComments.Where(pc => pc.Pid == vPost.id).Count();
                                postList.Add(vPost);
                            });
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { postList })), new commParameter(preq.loginUserId, preq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }


        [HttpPost]
        public async Task<string> AddAsync([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        addRequest preq = JsonConvert.DeserializeObject<addRequest>(reqStr);
                        if (!string.IsNullOrEmpty(preq.content))
                        {
                            Models.DB.User user = null;
                            Models.DB.Post post= new Post();
                            var userId = 0;
                            using (fnsContext db = new fnsContext())
                            {
                                var isLogin = true;
                                if (Int32.TryParse(preq.loginUserId, out userId))
                                {
                                    user = db.User.SingleOrDefault(u => u.Id == userId);
                                }
                                else
                                    isLogin = false;
                                if (isLogin && user != null)
                                {
                                    return JsonConvert.SerializeObject(new ResponseCommon("0003", "请先登录！", null, new commParameter(preq.loginUserId, preq.transId)));
                                }

                                post = new Post()
                                {
                                    Content = preq.content,
                                    PicUrlList = preq.picUrlList,
                                    Status = (int)PostStatusEnum.Normal,
                                    InsDt = DateTime.Now
                                };
                                

                                await db.Post.AddAsync(post);
                                await db.SaveChangesAsync();
                            }
                            return JsonConvert.SerializeObject(new ResponseCommon("0000", "发帖成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { user = post.ToViewModel(userId, settings.Value.ServerPath) })), new commParameter(preq.loginUserId, preq.transId)));
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0002", "帖子内容不能为空！", null, new commParameter(preq.loginUserId, preq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        [HttpPost]
        public async Task<string> DeleteAsync([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        idRequest preq = JsonConvert.DeserializeObject<idRequest>(reqStr);

                        var uId = 0;
                        Int32.TryParse(preq.loginUserId, out uId);
                        using (fnsContext db = new fnsContext())
                        {
                            var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                            if (user == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0002", "请先登录！", null, new commParameter(preq.loginUserId, preq.transId)));
                            }
                            var post = await db.Post.SingleOrDefaultAsync(p => p.Id == preq.id && p.Uid == uId);
                            if (post == null)
                            {
                                return JsonConvert.SerializeObject(new ResponseCommon("0003", "找不到该帖子！", null, new commParameter(preq.loginUserId, preq.transId)));
                            }
                            post.Status = (int)PostStatusEnum.Deleted;
                            db.Post.Update(post);
                            await db.SaveChangesAsync();
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "删除成功！", null, new commParameter(preq.loginUserId, preq.transId)));
                        
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
