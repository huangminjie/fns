using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.API.Request.News;
using fns.Models.API.Response.News;
using fns.Models.DB;
using fns.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using fns.Utils.API;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using fns.Models.Global;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {

        public NewsController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }
        

        [HttpPost("GetById")]
        public async Task<string> GetById([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        newsRequest rreq = JsonConvert.DeserializeObject<newsRequest>(reqStr);
                        newsResponse news = new newsResponse();
                        var model = await db.News.SingleOrDefaultAsync(n => n.Id == rreq.id);
                        if (model == null)
                            return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该文章！", null, new commParameter("", "")));

                        model.ViewCount = (model.ViewCount ?? 0) + 1;
                        db.News.Update(model);
                        db.SaveChanges();
                        news = model.ToViewModel(settings.Value.ServerPath);
                        #region 获取category name
                        if (string.IsNullOrEmpty(news.cName))
                        {
                            news.cName = db.Category.SingleOrDefault(o => o.Id == model.Cid)?.Name;
                        }
                        #endregion

                        #region 判断是否被用户收藏
                        news.isCollection = false;
                        Int32.TryParse(rreq.loginUserId, out int uId);
                        var user = db.User.SingleOrDefault(u => u.Id == uId);
                        if (user != null)
                        {
                            var collections = !string.IsNullOrEmpty(user.Collections) ? JsonConvert.DeserializeObject<List<int>>(user.Collections) : new List<int>();
                            if (collections.Contains(model.Id))
                            {
                                news.isCollection = true;
                            }
                        }
                        #endregion

                        #region 获取正常评论
                        var comments = db.Comment.Where(c => c.NId == news.id && c.Status != (int)CommentStatusEnum.Illegal).Select(o=>o.ToViewModel()).ToList();
                        news.comments= new List<CommentResponse>();
                        news.comments.AddRange(comments);
                        #endregion
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { news = news })), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        // GET api/values
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
                        newsFilterRequest rreq = JsonConvert.DeserializeObject<newsFilterRequest>(reqStr);
                        DateTime dt = DateTime.Now;
                        List<News> list = new List<News>();
                        List<newsResponse> newsList = new List<newsResponse>();
                        News news = await db.News.SingleOrDefaultAsync(o => o.Id == (rreq.id ?? 0));
                        if (news != null)
                        {
                            dt = news.InsDt ?? DateTime.Now;
                        }
                        rreq.title = string.IsNullOrEmpty(rreq.title) ? "" : rreq.title;
                        rreq.auth = string.IsNullOrEmpty(rreq.auth) ? "" : rreq.auth;
                        //上拉获取历史数据
                        if (rreq.op == 0)
                        {
                            list = await db.News.Where(n => n.Auth.Contains(rreq.auth) && n.Title.Contains(rreq.title) && n.Cid == rreq.cid && n.InsDt < dt).OrderByDescending(o => o.InsDt).Take(rreq.ps).ToListAsync();
                        }
                        //下拉获取最新数据
                        else
                        {
                            list = await db.News.Where(n => n.Auth.Contains(rreq.auth) && n.Title.Contains(rreq.title) && n.Cid == rreq.cid && n.InsDt > dt).OrderBy(o => o.InsDt).Take(rreq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                        }
                        var categories = db.Category.ToList();
                        var comments = db.Comment.Where(c => c.Status != (int)CommentStatusEnum.Illegal).ToList();
                        list.ForEach(l =>
                        {
                            var vNews = l.ToViewModel(settings.Value.ServerPath);
                            #region 获取category name
                            if (string.IsNullOrEmpty(vNews.cName))
                            {
                                vNews.cName = categories.SingleOrDefault(o => o.Id == vNews.cid)?.Name;
                            }
                            #endregion

                            #region 获取正常评论
                            vNews.comments = new List<CommentResponse>();
                            vNews.comments.AddRange(comments.Where(c => c.NId == vNews.id).Select(o=> o.ToViewModel()).ToList());
                            #endregion
                            newsList.Add(vNews);
                        });
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { newsList = newsList })), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        [HttpPost("Collection")]
        public async Task<string> Collection([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        collectionRequest rreq = JsonConvert.DeserializeObject<collectionRequest>(reqStr);
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);
                        var user = await db.User.SingleOrDefaultAsync(u=>u.Id == uId);
                        var returnMsg = "";
                        var returnCode = "0000";
                        if (user != null)
                        {
                            var collections = !string.IsNullOrEmpty(user.Collections) ? JsonConvert.DeserializeObject<List<int>>(user.Collections) : new List<int>();

                            if (collections.Any(c => c == rreq.nId))
                            {
                                collections.Remove(rreq.nId);
                                returnMsg = "取消收藏！";
                            }
                            else
                            {
                                collections.Add(rreq.nId);
                                returnMsg = "收藏成功！";
                            }
                            user.Collections = JsonConvert.SerializeObject(collections);
                            db.User.Update(user);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            returnMsg = "请先登录！";
                            returnCode = "0002";
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon(returnCode, returnMsg, null, new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        [HttpPost("GetCollections")]
        public async Task<string> GetCollections([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        getCollectionRequest rreq = JsonConvert.DeserializeObject<getCollectionRequest>(reqStr);

                        List<int> collections = new List<int>();
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);
                        var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                        if (user != null)
                        {
                            collections = !string.IsNullOrEmpty(user.Collections) ? JsonConvert.DeserializeObject<List<int>>(user.Collections) : new List<int>();
                        }

                        DateTime dt = DateTime.Now;
                        List<News> list = new List<News>();
                        List<newsResponse> newsList = new List<newsResponse>();
                        News news = await db.News.SingleOrDefaultAsync(o => o.Id == (rreq.id ?? 0));
                        if (news != null)
                        {
                            dt = news.InsDt ?? DateTime.Now;
                        }
                        //上拉获取历史数据
                        if (rreq.op == 0)
                        {
                            list = await db.News.Where(n => collections.Contains(n.Id) && n.InsDt < dt).OrderByDescending(o => o.InsDt).Take(rreq.ps).ToListAsync();
                        }
                        //下拉获取最新数据
                        else
                        {
                            list = await db.News.Where(n => collections.Contains(n.Id)  && n.InsDt > dt).OrderBy(o => o.InsDt).Take(rreq.ps).OrderByDescending(o => o.InsDt).ToListAsync();
                        }
                        var categories = db.Category.ToList();
                        var comments = db.Comment.Where(c => c.Status != (int)CommentStatusEnum.Illegal).ToList();
                        list.ForEach(l =>
                        {
                            var vNews = l.ToViewModel(settings.Value.ServerPath);
                            #region 获取category name
                            if (string.IsNullOrEmpty(vNews.cName))
                            {
                                vNews.cName = categories.SingleOrDefault(o => o.Id == vNews.cid)?.Name;
                            }
                            #endregion
                            #region 获取正常评论
                            vNews.comments = new List<CommentResponse>();
                            vNews.comments.AddRange(comments.Where(c => c.NId == vNews.id).Select(o => o.ToViewModel()).ToList());
                            #endregion
                            newsList.Add(vNews);
                        });
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { newsList = newsList })), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        [HttpPost("GetHistoricalRecords")]
        public string GetHistoricalRecords([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        getHistoryRequest rreq = JsonConvert.DeserializeObject<getHistoryRequest>(reqStr);
                        
                        List<newsResponse> newsList = new List<newsResponse>();
                        var news = db.News.Where(n => rreq.nIds.Contains(n.Id));
                        var categories = db.Category.ToList();
                        var comments = db.Comment.Where(c => c.Status != (int)CommentStatusEnum.Illegal).ToList();
                        rreq.nIds.ForEach(id=>{
                            var thisNews = news.SingleOrDefault(n => n.Id == id);
                            if (thisNews != null)
                            {
                                var vNews = thisNews.ToViewModel(settings.Value.ServerPath);
                                #region 获取category name
                                if (string.IsNullOrEmpty(vNews.cName))
                                {
                                    vNews.cName = categories.SingleOrDefault(o => o.Id == vNews.cid)?.Name;
                                }
                                #endregion
                                #region 获取正常评论
                                vNews.comments = new List<CommentResponse>();
                                vNews.comments.AddRange(comments.Where(c => c.NId == vNews.id).Select(o => o.ToViewModel()).ToList());
                                #endregion
                                newsList.Add(vNews);
                            }
                        });
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { newsList })), new commParameter(rreq.loginUserId, rreq.transId)));
                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }


        [HttpPost("Comment")]
        public async Task<string> Comment([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        commentRequest rreq = JsonConvert.DeserializeObject<commentRequest>(reqStr);
                        var uId = 0;
                        Int32.TryParse(rreq.loginUserId, out uId);
                        var user = await db.User.SingleOrDefaultAsync(u => u.Id == uId);
                        var news = await db.News.SingleOrDefaultAsync(n => n.Id == rreq.nId);
                        var returnMsg = "";
                        var returnCode = "0000";
                        if (user != null)
                        {
                            if (news != null)
                            {
                                if (!string.IsNullOrEmpty(rreq.comment))
                                {
                                    Comment comment = new Comment() {
                                        UId = user.Id,
                                        NId = news.Id,
                                        Content = rreq.comment,
                                        Status = (int)CommentStatusEnum.Normal,
                                        InsDt =DateTime.Now
                                    };
                                    db.Comment.Add(comment);
                                    news.CommentCount = (news.CommentCount ?? 0)  + 1;
                                    db.News.Update(news);
                                    await db.SaveChangesAsync();
                                    returnMsg = "评论成功！";
                                }
                                else
                                {
                                    returnMsg = "评论不能为空！";
                                    returnCode = "0004";
                                }
                            }
                            else {

                                returnMsg = "找不到该新闻！";
                                returnCode = "0003";
                            }
                        }
                        else
                        {
                            returnMsg = "请先登录！";
                            returnCode = "0002";
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon(returnCode, returnMsg, null, new commParameter(rreq.loginUserId, rreq.transId)));
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
