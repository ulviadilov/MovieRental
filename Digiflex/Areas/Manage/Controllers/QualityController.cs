using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class QualityController : Controller
    {
        private readonly DigiflexContext _context;

        public QualityController(DigiflexContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Qualities.AsQueryable();
            var paginatedList = PaginatedList<Quality>.Create(query, 5, page);
            return View(paginatedList);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Quality quality)
        {
            if (!ModelState.IsValid) return View(quality);
            _context.Qualities.Add(quality);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Quality quality = _context.Qualities.Find(id);
            if (quality is null) return NotFound();
            return View(quality);
        }



        [HttpPost]
        public IActionResult Update(Quality quality)
        {
            if (!ModelState.IsValid) return View(quality);
            Quality existQuality = _context.Qualities.FirstOrDefault(x => x.Id == quality.Id);
            if (existQuality is null) return NotFound();
            existQuality.QualityName = quality.QualityName;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Quality quality = _context.Qualities.Find(id);
            if (quality is null) return NotFound();
            _context.Qualities.Remove(quality);
            _context.SaveChanges();
            return Ok();
        }
    }
}
