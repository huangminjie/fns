﻿using System;
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
                        list.ForEach(l =>
                        {
                            newsList.Add(l.ToViewModel(settings.Value.ServerPath));
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


    }
}
