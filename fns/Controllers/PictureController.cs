using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using fns.Models.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using fns.Models.Global;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Controllers
{
    [Authorize]
    public class PictureController : BaseController
    {
        public PictureController(IHostingEnvironment environment, IOptions<AppSettings> settings) : base(environment, settings)
        {

        }

        public class UploadFileData
        {
            public IFormFile file { get; set; }
            public string type { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> UploadPicture()
        {
            try
            {
                var allFiles = Request.Form.Files;
                var type = Request.Form["type"];
                var root = environment.WebRootPath;
                if (allFiles != null)
                {
                    var urls = new List<string>();
                    foreach (var file in allFiles)
                    {
                        var extension = Path.GetExtension(file.FileName);
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
                            await file.CopyToAsync(stream);
                        }
                        urls.Add($"/upload/{type}/{fileName}");
                    }
                    return Ok(new Response(true, "", urls));
                }
                else
                {
                    return Ok("无法获取上传文件!");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
