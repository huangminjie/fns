﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using fns.Models.DB;
using fns.Models.API;
using fns.Utils;
using Newtonsoft.Json;
using fns.Models.API.Request;
using fns.Models.API.Response.Banner;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class BannerController : Controller
    {
        private FinancialNewsContext db = new FinancialNewsContext();
        // GET: api/values
        [HttpPost("GetBanners")]
        public string GetBanners([FromBody]RequestCommon req)
        {
            try
            {
                if (req != null && req.d != null)
                {
                    var reqStr = DESUtil.DecryptCommonParam(req.d);
                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        RequestBase rreq = JsonConvert.DeserializeObject<RequestBase>(reqStr);
                        List<bannerResponse> banners = new List<bannerResponse>();
                        db.Banner.ToList().ForEach(o => {
                            banners.Add(new bannerResponse()
                            {
                                linkUrl = o.LinkUrl,
                                picUrl = o.PicUrl,
                                type = o.Type ?? (int)BannerRedirectTypeEnum.In
                            });
                        });
                        return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", DESUtil.EncryptCommonParam(JsonConvert.SerializeObject(new { banners = banners })), new commParameter(rreq.loginUserId, rreq.transId)));

                    }
                }
                return JsonConvert.SerializeObject(new ResponseCommon("0001", "请求无效, 参数异常！", null, new commParameter("", "")));
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
