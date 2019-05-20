using fns.Models.Admin;
using fns.Models.API;
using fns.Models.DB;
using fns.Models.Global;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.API
{
    [Route("api/[controller]")]
    public class PictureController : BaseController
    {
        public PictureController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }

        [HttpPost("UploadPicture")]
        public async Task<string> UploadPicture()
        {
            try
            {
                var firstFile = Request.Form.Files[0];
                var type = Request.Form["type"];
                var root = environment.WebRootPath;
                if (firstFile != null)
                {
                    var extension = Path.GetExtension(firstFile.FileName);
                    var guid = Guid.NewGuid().ToString();
                    var fileName = guid + extension;
                    string folderName = $"{root}/upload/{type}";
                    if (!System.IO.Directory.Exists(folderName))
                    {
                        System.IO.Directory.CreateDirectory(folderName);
                    }
                    var filePath = $"{root}/upload/{type}/{fileName}";
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        await firstFile.CopyToAsync(stream);
                    }
                    string url = $"/upload/{type}/{fileName}";
                    
                    return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", new { severpath = settings.Value.ServerPath, url = url }, new commParameter("", "")));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ResponseCommon("0001", "无法获取上传文件", null, new commParameter("", "")));
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }


        [HttpPost("UploadPictures")]
        public async Task<string> UploadPictures()
        {
            try
            {
                var firstFiles = Request.Form.Files;
                var type = Request.Form["type"];
                var root = environment.WebRootPath;
                List<string> urls = new List<string>();
                if (firstFiles != null && firstFiles.Count > 0)
                {
                    foreach (var firstFile in firstFiles)
                    {
                        //var extension = Path.GetExtension(firstFile.FileName);
                        var extension = DateTime.Now.ToString("yyyyMMddHHmmss");
                        var guid = Guid.NewGuid().ToString();
                        var fileName = guid + extension;
                        string folderName = $"{root}/upload/{type}";
                        if (!System.IO.Directory.Exists(folderName))
                        {
                            System.IO.Directory.CreateDirectory(folderName);
                        }
                        var filePath = $"{root}/upload/{type}/{fileName}";
                        using (FileStream stream = new FileStream(filePath, FileMode.Create))
                        {
                            await firstFile.CopyToAsync(stream);
                        }
                        string url = $"/upload/{type}/{fileName}";
                        urls.Add(url);
                    }

                    return JsonConvert.SerializeObject(new ResponseCommon("0000", "成功！", new { severpath = settings.Value.ServerPath, urls = urls }, new commParameter("", "")));
                }
                else
                {
                    return JsonConvert.SerializeObject(new ResponseCommon("0001", "无法获取上传文件", null, new commParameter("", "")));
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new ResponseCommon("0001", ex.Message, null, new commParameter("", "")));
            }
        }
    }
}
