using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ServiceController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Services.AsQueryable();
            var paginatedList = PaginatedList<Service>.Create(query, 5, page);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Service service)
        {
            if(!ModelState.IsValid) return View();

            if (service.IconImageFile.ContentType != "image/png" && service.IconImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("IconImageFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (service.IconImageFile.Length > 3145728)
            {
                ModelState.AddModelError("IconImageFile", "You can only upload files under 3mb size");
                return View();
            }

            service.IconImageUrl = service.IconImageFile.SaveImage(_env.WebRootPath, "uploads/service");

            _context.Services.Add(service);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Service service = _context.Services.Find(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpPost]
        public IActionResult Update(Service service)
        {
            if (!ModelState.IsValid) return View();
            Service existService = _context.Services.FirstOrDefault(x => x.Id == service.Id);
            if (existService == null) return NotFound();
            if (service.IconImageFile is not null)
            {
                if (service.IconImageFile.ContentType != "image/png" && service.IconImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("IconImageFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (service.IconImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("IconImageFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/service", existService.IconImageUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existService.IconImageUrl = service.IconImageFile.SaveImage(_env.WebRootPath , "uploads/service");
            }
            existService.Title = service.Title;
            existService.Desc = service.Desc;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Delete(int id)
        {
            Service service = _context.Services.Find(id);
            if (service == null) return NotFound();
            _context.Services.Remove(service);
            _context.SaveChanges();
            return Ok();
        }
        
    }
    
}

