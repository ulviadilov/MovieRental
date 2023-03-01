using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class PaymentPlanController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public PaymentPlanController(DigiflexContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.PaymentPlans.ToList());
        }

        public IActionResult Update(int id)
        {
            PaymentPlan plan = _context.PaymentPlans.Find(id);
            if (plan == null) return NotFound();
            return View(plan);
        }

        [HttpPost]
        public IActionResult Update(PaymentPlan plan)
        {
            if(!ModelState.IsValid) return View();
            PaymentPlan existPlan = _context.PaymentPlans.FirstOrDefault(x => x.Id == plan.Id);
            if (existPlan == null) return NotFound();
            if (plan.BackgroundVideoFile is not null)
            {
                if (plan.BackgroundVideoFile.ContentType != "video/mp4")
                {
                    ModelState.AddModelError("BackgroundVideoFile", "You can only upload mp4 format files");
                    return View();
                }
                string deletePath = Path.Combine(_env.WebRootPath, "uploads/movievideo", existPlan.BackgroudVideoUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existPlan.BackgroudVideoUrl = plan.BackgroundVideoFile.SaveImage(_env.WebRootPath, "uploads/movievideo");
            }

            if (plan.IconFile is not null)
            {
                if (plan.IconFile.ContentType != "image/png" && plan.IconFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("IconFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (plan.IconFile.Length > 3145728)
                {
                    ModelState.AddModelError("IconFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/plan", existPlan.IconUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existPlan.IconUrl = plan.IconFile.SaveImage(_env.WebRootPath, "uploads/plan");

            }

            existPlan.PlanTitle = plan.PlanTitle;
            existPlan.Price = plan.Price;
            existPlan.Title = plan.Title;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
    }
}
