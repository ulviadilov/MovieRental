using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Sliders.AsQueryable();
            var paginatedList = PaginatedList<Slider>.Create(query, 5, page);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult Create(Slider slider) 
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            if (!ModelState.IsValid) return View();
            if (slider.PosterImageFile.ContentType != "image/png" && slider.PosterImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("PosterImageFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (slider.PosterImageFile.Length > 3145728)
            {
                ModelState.AddModelError("PosterImageFile", "You can only upload files under 3mb size");
                return View();
            }
            slider.PosterImageUrl = slider.PosterImageFile.SaveImage(_env.WebRootPath, "uploads/slider");

            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return NotFound();
            return View(slider);
        }


        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();

            if (!ModelState.IsValid) return View();
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.Id == slider.Id);
            if (existSlider == null) return NotFound();
            if (slider.PosterImageFile is not null)
            {
                if (slider.PosterImageFile.ContentType != "image/png" && slider.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (slider.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImageFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath , "uploads/slider" , existSlider.PosterImageUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existSlider.PosterImageUrl = slider.PosterImageFile.SaveImage(_env.WebRootPath , "uploads/slider");
            }

            existSlider.TraillerUrl = slider.TraillerUrl;
            existSlider.ImdbScore = slider.ImdbScore;
            existSlider.MovieName = slider.MovieName;
            existSlider.ReleaseYear = slider.ReleaseYear;
            existSlider.GenreId = slider.GenreId;
            existSlider.QualityId= slider.QualityId;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            if (slider == null) return NotFound();
            string deletePath = Path.Combine(_env.WebRootPath , "uploads/slider" , slider.PosterImageUrl);
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            _context.Sliders.Remove(slider);
            _context.SaveChanges();
            return Ok();
        }
    }
}
