using Digiflex.DAL;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Digiflex.Controllers
{
    public class PaymentController : Controller
    {
        private readonly DigiflexContext _context;

        public PaymentController(DigiflexContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Index(PaymentViewModel paymentVM)
        //{
        //    if(!ModelState.IsValid) return View();

        //}
    }
}
