using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.API;
using fns.Models.API.Request;
using fns.Models.DB;
using fns.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using fns.Models.API.Response.Splash;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class SplashController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();
        // GET: api/values
        [HttpGet]
        public string Get([FromQuery]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        RequestBase rreq = JsonConvert.DeserializeObject<RequestBase>(reqStr);
                        List<splashResponse> splashes = new List<splashResponse>();
                        db.Splash.ToList().ForEach(o => {
                            splashes.Add(new splashResponse()
                            {
                                id= o.Id,
                                redirectUrl = o.RedirectUrl,
                                picUrl = o.PicUrl,
                                duration = o.Duration ?? 0
                            });
                        });
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { splashes = splashes })), new commParameter(rreq.loginUserId, rreq.transId)));

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
