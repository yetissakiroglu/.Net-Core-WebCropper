using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace WebCropper.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult CustomCrop()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomCrop(string filename, IFormFile blob)
        {
            try
            {
                using (var image = Image.Load(blob.OpenReadStream()))
                {
                    string systemFileExtenstion = filename.Substring(filename.LastIndexOf('.'));

                    image.Mutate(x => x.Resize(180, 180));
                    var newfileName180 = GenerateFileName("Photo_180_180_", systemFileExtenstion);
                    var filepath160 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName180}";
                    image.Save(filepath160);

                    var newfileName200 = GenerateFileName("Photo_200_200_", systemFileExtenstion);
                    var filepath200 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName200}";
                    image.Mutate(x => x.Resize(200, 200));
                    image.Save(filepath200);

                    var newfileName32 = GenerateFileName("Photo_32_32_", systemFileExtenstion);
                    var filepath32 = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")).Root + $@"\{newfileName32}";
                    image.Mutate(x => x.Resize(32, 32));
                    image.Save(filepath32);

                }

                return Json(new { Message = "OK" });
            }
            catch (Exception)
            {
                return Json(new { Message = "ERROR" });
            }
        }

        public string GenerateFileName(string fileTypeName, string fileextenstion)
        {
            if (fileTypeName == null) throw new ArgumentNullException(nameof(fileTypeName));
            if (fileextenstion == null) throw new ArgumentNullException(nameof(fileextenstion));
            return $"{fileTypeName}_{DateTime.Now:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{fileextenstion}";
        }
    }
}