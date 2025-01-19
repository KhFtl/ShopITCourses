using Microsoft.AspNetCore.Mvc;

namespace ShopITCourses.Controllers
{
    public class CkeditorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CkeditorController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string Index()
        {
            return "Hello i`m uploader for CKEditor";
        }

        [HttpPost]
        public JsonResult Upload(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            { 
                var filename = upload.FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "files", filename);
                var str = new FileStream(path, FileMode.Create);
                upload.CopyToAsync(str);
                var url = $"{"/files/"}{filename}";
                return Json(new { uploaded = true, url });
            }
            return Json(new { path = "/files/" });
        }

        [HttpGet]
        public async Task<IActionResult> FileBrowser(IFormFile upload)
        {
            var path = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "files"));
            ViewBag.FilesUploads = path.GetFiles();
            return View("FileBrowser");
        }

        [HttpGet]
        public JsonResult Delete(string name)
        {
            var path = new DirectoryInfo(Path.Combine(_webHostEnvironment.WebRootPath, "files"));
            var file = Path.Combine(path.ToString(), name);
            try
            {
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                    return Json(new { success = true, message = "Файл успішно видалено." });
                }
                return Json(new { success = false, message = "Файл не знайдено." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Сталася помилка: {ex.Message}" });
            }
        }
    }
}
