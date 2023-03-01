using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SettingController : Controller
    {
        private readonly DigiflexContext _context;

        public SettingController(DigiflexContext context)
        {
            _context = context;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Settings.AsQueryable();
            var paginatedList = PaginatedList<Setting>.Create(query, 5, page);
            return View(paginatedList);
        }

        public IActionResult Update(int id)
        {
            Setting setting = _context.Settings.Find(id);
            if (setting == null) return NotFound();
            return View(setting);
        }

        [HttpPost]
        public IActionResult Update(Setting setting)
        {
            Setting existSetting = _context.Settings.FirstOrDefault(x => x.Id == setting.Id);
            if (existSetting == null) return NotFound();
            existSetting.Value= setting.Value;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
