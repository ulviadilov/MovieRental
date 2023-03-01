using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        private readonly DigiflexContext _context;

        public GenreController(DigiflexContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Genres.AsQueryable();
            var paginatedList = PaginatedList<Genre>.Create(query, 5, page);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if(!ModelState.IsValid) return View();
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Genre genre = _context.Genres.Find(id);
            if (genre is null) return NotFound();
            return View(genre);
        }



        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            if (!ModelState.IsValid) return View(genre);
            Genre existGenre = _context.Genres.FirstOrDefault(x => x.Id == genre.Id);
            if (existGenre is null) return NotFound();
            existGenre.Name = genre.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Genre genre = _context.Genres.Find(id);
            if (genre is null) return NotFound();
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return Ok();
        }
    }
}
