using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TestimonialController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public TestimonialController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Testimonials.ToList());
        }

        //public IActionResult Create() 
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Create(Testimonial testimonial)
        //{
        //    if (!ModelState.IsValid) return View();
        //    if (testimonial.AvatarFile.ContentType != "image/png" && testimonial.AvatarFile.ContentType != "image/jpeg")
        //    {
        //        ModelState.AddModelError("AvatarFile", "You can only upload png or jpeg format files");
        //        return View();
        //    }

        //    if (testimonial.AvatarFile.Length > 3145728)
        //    {
        //        ModelState.AddModelError("AvatarFile", "You can only upload files under 3mb size");
        //        return View();
        //    }

        //    if (testimonial.ImageFile.ContentType != "image/png" && testimonial.ImageFile.ContentType != "image/jpeg")
        //    {
        //        ModelState.AddModelError("ImageFile", "You can only upload png or jpeg format files");
        //        return View();
        //    }

        //    if (testimonial.ImageFile.Length > 3145728)
        //    {
        //        ModelState.AddModelError("ImageFile", "You can only upload files under 3mb size");
        //        return View();
        //    }

        //    if (testimonial.LogoFile.ContentType != "image/png" && testimonial.LogoFile.ContentType != "image/jpeg")
        //    {
        //        ModelState.AddModelError("LogoFile", "You can only upload png or jpeg format files");
        //        return View();
        //    }

        //    if (testimonial.LogoFile.Length > 3145728)
        //    {
        //        ModelState.AddModelError("LogoFile", "You can only upload files under 3mb size");
        //        return View();
        //    }

        //    testimonial.ImageUrl = testimonial.ImageFile.SaveImage(_env.WebRootPath, "uploads/testimonial");
        //    testimonial.LogoUrl = testimonial.LogoFile.SaveImage(_env.WebRootPath, "uploads/testimonial");
        //    testimonial.AvatarUrl = testimonial.AvatarFile.SaveImage(_env.WebRootPath, "uploads/testimonial");
        //    _context.Testimonials.Add(testimonial);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");
        //}


        public IActionResult Update(int id)
        {
            Testimonial testimonial = _context.Testimonials.Find(id);
            if (testimonial == null) return NotFound();
            return View(testimonial);
        }

        [HttpPost]
        public IActionResult Update(Testimonial testimonial)
        {
            if(!ModelState.IsValid) return View(testimonial);
            Testimonial existTestimonial = _context.Testimonials.FirstOrDefault(x => x.Id == testimonial.Id);
            if(existTestimonial is null) return NotFound();
            if(testimonial.AvatarFile is not null)
            {
                if (testimonial.AvatarFile.ContentType != "image/png" && testimonial.AvatarFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("AvatarFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (testimonial.AvatarFile.Length > 3145728)
                {
                    ModelState.AddModelError("AvatarFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath , "uploads/testimonial" , existTestimonial.AvatarUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath); 
                }

                existTestimonial.AvatarUrl = testimonial.AvatarFile.SaveImage(_env.WebRootPath  , "uploads/testimonial");
            }

            if (testimonial.ImageFile is not null)
            {
                if (testimonial.ImageFile.ContentType != "image/png" && testimonial.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (testimonial.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/testimonial", existTestimonial.ImageUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existTestimonial.ImageUrl = testimonial.ImageFile.SaveImage(_env.WebRootPath, "uploads/testimonial");
            }

            if (testimonial.LogoFile is not null)
            {
                if (testimonial.LogoFile.ContentType != "image/png" && testimonial.LogoFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("LogoFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (testimonial.LogoFile.Length > 3145728)
                {
                    ModelState.AddModelError("LogoFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/testimonial", existTestimonial.LogoUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existTestimonial.LogoUrl = testimonial.LogoFile.SaveImage(_env.WebRootPath, "uploads/testimonial");
            }

            existTestimonial.Title = testimonial.Title;
            existTestimonial.Desc = testimonial.Desc;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
