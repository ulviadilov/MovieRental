using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DeviceController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public DeviceController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Devices.AsQueryable();
            var paginatedList = PaginatedList<Device>.Create(query, 5, page);
            return View(paginatedList);
        }


        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Device device)
        {
            if (!ModelState.IsValid) return View();

            if (device.IconFile.ContentType != "image/png" && device.IconFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("IconFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (device.IconFile.Length > 3145728)
            {
                ModelState.AddModelError("IconFile", "You can only upload files under 3mb size");
                return View();
            }

            device.IconUrl = device.IconFile.SaveImage(_env.WebRootPath, "uploads/device");

            _context.Devices.Add(device);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int id) 
        {
            Device device = _context.Devices.Find(id);
            if (device == null) return NotFound();
            return View(device);
        }

        [HttpPost]
        public IActionResult Update(Device device)
        {
            if (!ModelState.IsValid) return View();
            Device existDevice = _context.Devices.FirstOrDefault(x => x.Id == device.Id);
            if (existDevice == null) return NotFound();
            if (existDevice.IconFile is not null)
            {
                if (device.IconFile.ContentType != "image/png" && device.IconFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("IconFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (device.IconFile.Length > 3145728)
                {
                    ModelState.AddModelError("IconFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/service", existDevice.IconUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existDevice.IconUrl = device.IconFile.SaveImage(_env.WebRootPath, "uploads/device");
            }
            existDevice.Title = device.Title;
            existDevice.Description = device.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Device device = _context.Devices.Find(id);
            if (device == null) return NotFound();
            _context.Devices.Remove(device);
            _context.SaveChanges();
            return Ok();
        }
    }
}
