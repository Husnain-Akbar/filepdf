using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using filepdf.Models;
using filepdf.Data;
using Microsoft.AspNetCore.Hosting;
using filepdf.Models.ViewModels;
using System.IO;

namespace filepdf.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnv;
        private readonly ApplicationDbContext _context;

        public HomeController(IHostingEnvironment hostingEnv, ApplicationDbContext context)
        {
            _hostingEnv = hostingEnv;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EngineerVM engineerVM)
        {
            string filepath = FilePath(engineerVM);
            
                Engineer engineer = new Engineer();
                engineer.Name = engineerVM.Name;
                engineer.FilePath = filepath;

                _context.Engineers.Add(engineer);
                await _context.SaveChangesAsync();
            //}
            //else
            //{

            //}
            return View();
        }
    

        public string FilePath(EngineerVM engineerVM)
        {
            var filePath ="";
            if (engineerVM.File != null)
            {
                //upload files to wwwroot
                 var fileName = Path.GetFileName(engineerVM.File.FileName);
                //judge if it is pdf file
                string ext = Path.GetExtension(engineerVM.File.FileName);
                if (ext.ToLower() != ".pdf" )
                {
                    return "";
                }
                 filePath = Path.Combine(_hostingEnv.WebRootPath, "myFiles", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                     engineerVM.File.CopyToAsync(fileSteam);
                }


            }
            return filePath;

        }
        public IActionResult Privacy()
        {
            var model= _context.Engineers.ToList();
            return View(model);
        }

        public IActionResult DownloadFile(string filePath)
        {

            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            string fileName = "myfile.pdf";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            //For preview pdf and the download it use below code
            // var stream = new FileStream(filePath, FileMode.Open);
            //return new FileStreamResult(stream, "application/pdf");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
