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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class NewsController : BaseController
    {

        public NewsController(IOptions<Models.Global.AppSettings> settings) : base(settings)
        {

        }

        public static string ServerPath = "";
        private FinancialNewsContext db = new FinancialNewsContext();

        [HttpPost("GetById")]
        public string GetById([FromBody]RequestCommon req)
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
                        var model = db.News.SingleOrDefault(n => n.Id == rreq.id);
                        if (model == null)
                            return JsonConvert.SerializeObject(new ResponseCommon("0002", "找不到该文章！", null, new commParameter("", "")));
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
        public string GetList([FromBody] RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        newsFilterRequest rreq = JsonConvert.DeserializeObject<newsFilterRequest>(reqStr);
                        List<newsResponse> newsList = new List<newsResponse>();
                        var list = db.News.Where(n => n.Auth.Contains(rreq.auth) && n.Title.Contains(rreq.title) && n.Cid == rreq.cid).Skip((rreq.pi - 1) * rreq.ps).Take(rreq.ps).ToList();
                        list.ForEach(l =>
                        {
                            var news = l.ToViewModel(settings.Value.ServerPath);
                            newsList.Add(news);
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
