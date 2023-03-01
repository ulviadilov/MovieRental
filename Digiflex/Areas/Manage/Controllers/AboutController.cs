using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AboutController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public AboutController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Abouts.ToList());
        }


        public IActionResult Update(int id)
        {
            About about = _context.Abouts.Find(id);
            if (about == null) return NotFound();
            return View(about);
        }

        [HttpPost]
        public IActionResult Update(About about)
        {
            if (!ModelState.IsValid) return View(about);
            About existAbout = _context.Abouts.FirstOrDefault(x => x.Id == about.Id);
            if (about.ImageFile is not null)
            {
                if (about.ImageFile.ContentType != "image/png" && about.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (about.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath , "uploads/about" , existAbout.ImageUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existAbout.ImageUrl = about.ImageFile.SaveImage(_env.WebRootPath , "uploads/about");
            }
            existAbout.MainTitle = about.MainTitle;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
