using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ContactController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public ContactController(DigiflexContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Contacts.AsQueryable();
            var paginatedList = PaginatedList<Contact>.Create(query, 5, page);
            return View(paginatedList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            if (!ModelState.IsValid) return View();

            if (contact.IconFile.ContentType != "image/png" && contact.IconFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("IconFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (contact.IconFile.Length > 3145728)
            {
                ModelState.AddModelError("IconFile", "You can only upload files under 3mb size");
                return View();
            }

            contact.IconUrl = contact.IconFile.SaveImage(_env.WebRootPath, "uploads/contact");

            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Update(int id)
        {
            Contact contact = _context.Contacts.Find(id);
            if (contact == null) return NotFound();
            return View(contact);
        }

        [HttpPost]
        public IActionResult Update(Contact contact)
        {
            if (!ModelState.IsValid) return View();
            Contact existContact = _context.Contacts.FirstOrDefault(x => x.Id == contact.Id);
            if (existContact == null) return NotFound();
            if (existContact.IconFile is not null)
            {
                if (contact.IconFile.ContentType != "image/png" && contact.IconFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("IconFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (contact.IconFile.Length > 3145728)
                {
                    ModelState.AddModelError("IconFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/service", existContact.IconUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existContact.IconUrl = contact.IconFile.SaveImage(_env.WebRootPath, "uploads/contact");
            }
            existContact.Title = contact.Title;
            existContact.Description = contact.Description;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            Contact contact = _context.Contacts.Find(id);
            if (contact == null) return NotFound();
            _context.Contacts.Remove(contact);
            _context.SaveChanges();
            return Ok();
        }
    }
}
