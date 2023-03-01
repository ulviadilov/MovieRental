using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class LanguageController : Controller
    {
        private readonly DigiflexContext _context;

        public LanguageController(DigiflexContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Languages.AsQueryable();
            var paginatedList = PaginatedList<Language>.Create(query, 5, page);
            return View(paginatedList);
        }

        public IActionResult Create()
        {
            return View() ;
        }

        [HttpPost]
        public IActionResult Create(Language language)
        {
            if (!ModelState.IsValid) return View();
            _context.Languages.Add(language);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Language language = _context.Languages.Find(id);
            if (language is null) return NotFound();
            return View(language);
        }



        [HttpPost]
        public IActionResult Update(Language language)
        {
            if (!ModelState.IsValid) return View(language);
            Language existLanguage = _context.Languages.FirstOrDefault(x => x.Id == language.Id);
            if (existLanguage is null) return NotFound();
            existLanguage.LanguageName = language.LanguageName;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Language language = _context.Languages.Find(id);
            if (language is null) return NotFound();
            _context.Languages.Remove(language);
            _context.SaveChanges();
            return Ok();
        }
    }
}
