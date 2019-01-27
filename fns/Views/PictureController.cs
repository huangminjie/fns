using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace fns.Views
{
    public class PictureController : Controller
    {

        public PictureController(IHostingEnvironment environment)
        {
            this.environment = environment;
        }
        

        public class UploadFileData
        {
            public IFormFile file { get; set; }
        }
        private IHostingEnvironment environment { get; set; }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadPicture(UploadFileData data)
        {
            var allFiles = Request.Form.Files; // 多文件的话可以直接从 form 拿到完, 或则设置成 List<IFormFile> 就可以了
            var root = environment.WebRootPath;
            var extension = Path.GetExtension(data.file.FileName);
            var guid = Guid.NewGuid().ToString();
            var fullPath = $@"{root}\UploadPics\{guid + extension}";
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                await data.file.CopyToAsync(stream);
            }
            return Ok();
        }
    }
}
