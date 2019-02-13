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
using Microsoft.AspNetCore.Hosting;
using fns.Models.Global;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class SplashController : BaseController
    {
        public SplashController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }

        [HttpPost("GetSplash")]
        public string GetSplash([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        RequestBase rreq = JsonConvert.DeserializeObject<RequestBase>(reqStr);
                        splashResponse splash = new splashResponse();
                        var model = db.Splash.FirstOrDefault();
                        if (model != null)
                        {
                            splash = new splashResponse()
                            {
                                id = model.Id,
                                redirectUrl = model.RedirectUrl,
                                picUrl = settings.Value.ServerPath + model.PicUrl,
                                duration = model.Duration ?? 0
                            };
                        }
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { splash = splash })), new commParameter(rreq.loginUserId, rreq.transId)));

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
