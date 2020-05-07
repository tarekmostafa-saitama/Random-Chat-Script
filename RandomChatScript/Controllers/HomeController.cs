using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RandomChatScript.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;


        public HomeController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Upload")]
        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            string NewName = Guid.NewGuid().ToString();
            string path = null;
            if (file != null && file.Length > 0)
            {
                var folder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
                var pathString = Path.Combine(folder, NewName + file.FileName);
                file.CopyTo(new FileStream(pathString, FileMode.Create));
            }
            path = Path.Combine("https://localhost:44384/uploads", NewName + file.FileName);
            return Json(path);
        }
    }
}