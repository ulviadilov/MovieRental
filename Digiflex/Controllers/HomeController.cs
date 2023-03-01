using Digiflex.DAL;
using Digiflex.Models;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Digiflex.Controllers
{
    public class HomeController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(DigiflexContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult WaitRoom()
        {
            return View();
        }

        public IActionResult Index()
        {
            string userId = _userManager.GetUserId(HttpContext.User);
            AppUser user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user is not null && user.SubscriptionEndTime <= DateTime.Now)
            {
                user.IsSubscribed = false;
                _context.SaveChanges();
            }
            HomeViewModel homeVM = new HomeViewModel
            {
                Abouts = _context.Abouts.ToList(),
                Sliders = _context.Sliders.Include(x => x.Quality).Include(x => x.Genre).ToList(),
                Services = _context.Services.ToList(),
                Testimonials = _context.Testimonials.ToList(),
                PaymentPlans = _context.PaymentPlans.ToList(),
                FeaturedMovies = _context.Movies.Include(x => x.MovieGenres).ThenInclude(x => x.Genre).Include(x => x.Quality).ToList()
            };
            return View(homeVM);
        }

        public IActionResult Devices()
        {
            return View(_context.Devices.ToList());
        }

        public IActionResult Contacts()
        {
            return View(_context.Contacts.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult TermsAndAgreement()
        {
            return View();
        }

        public IActionResult LegalNotices()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
    }
}