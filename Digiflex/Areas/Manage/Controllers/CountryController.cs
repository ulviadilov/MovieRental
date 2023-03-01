using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CountryController : Controller
    {
        private readonly DigiflexContext _context;

        public CountryController(DigiflexContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Countries.AsQueryable();
            var paginatedCountry = PaginatedList<Country>.Create(query, 5, page);
            return View(paginatedCountry);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Country country)
        {
            if(!ModelState.IsValid) return View(country);
            _context.Countries.Add(country);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            Country country = _context.Countries.Find(id);
            if(country is null) return NotFound();
            return View(country);
        }



        [HttpPost]
        public IActionResult Update(Country country)
        {
            if (!ModelState.IsValid) return View(country);
            Country existcountry = _context.Countries.FirstOrDefault(x => x.Id == country.Id);
            if (existcountry is null) return NotFound();
            existcountry.Name = country.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Country country = _context.Countries.Find(id);
            if (country is null) return NotFound();
            _context.Countries.Remove(country);
            _context.SaveChanges();
            return Ok();
        }
    }
}
