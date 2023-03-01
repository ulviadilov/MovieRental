
using Digiflex.DAL;
using Digiflex.Models;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DigiflexContext _context;

        public DashboardController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager , DigiflexContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        public IActionResult Index()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel
            {
                Movies = _context.Movies.ToList(),
                Users= _context.Users.ToList(),
                Payments = _context.Payments.ToList(),
                Comments= _context.Comments.ToList()
            };
            return View(dashboardVM);
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser admin = new AppUser
            {
                UserName = "SuperAdmin",
                FullName = "Ulvi Adilov",
                ProfilePhotoUrl = "avatara.jpg",
                IsSubscribed = true,
                IsDeleted = false,
                PhoneNumber = "+994557908327"
            };

            var result = await _userManager.CreateAsync(admin, "Adilov7802");
            return Ok(result);
        }


        public async Task<IActionResult> CreateRole()
        {
            IdentityRole role1 = new IdentityRole("SuperAdmin");
            IdentityRole role2 = new IdentityRole("Admin");
            IdentityRole role3 = new IdentityRole("Member");
            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            await _roleManager.CreateAsync(role3);

            return Ok("Created");
        }


        public async Task<IActionResult> AddRole()
        {
            AppUser user = await _userManager.FindByNameAsync("SuperAdmin");

            await _userManager.AddToRoleAsync(user, "SuperAdmin");
            return Ok("Role added");
        }


    }
}
