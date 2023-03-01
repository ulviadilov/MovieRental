using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Digiflex.ViewModels;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Digiflex.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, DigiflexContext context, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _env = env;
        }
        public IActionResult Profile(string id)
        {
            AppUser user = _context.Users.Find(id);
            if (user is null) return NotFound();
            return View(user);
        }

        [HttpPost]
        public IActionResult Profile(AppUser user)
        {
            if(!ModelState.IsValid) return View();
            AppUser existUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if(existUser is null) return NotFound();
            if (user.ProfilePhotoFile is not null)
            {
                if(user.ProfilePhotoFile.ContentType != "image/png" && user.ProfilePhotoFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ProfilePhotoFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (user.ProfilePhotoFile.Length > 3145728)
                {
                    ModelState.AddModelError("ProfilePhotoFile", "You can only upload files under 3mb size");
                    return View();
                }

                if (existUser.ProfilePhotoUrl is not null)
                {
                    string deletePath = Path.Combine(_env.WebRootPath, "uploads/user", existUser.ProfilePhotoUrl);
                    if (System.IO.File.Exists(deletePath))
                    {
                        System.IO.File.Delete(deletePath);
                    }
                }
                existUser.ProfilePhotoUrl = user.ProfilePhotoFile.SaveImage(_env.WebRootPath, "uploads/user");
            }
            existUser.FullName = user.FullName;
            existUser.PhoneNumber = user.PhoneNumber;
            existUser.Email = user.Email;
            existUser.UserName = user.UserName;
            _context.SaveChanges();
            return RedirectToAction("index" , "home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterViewModel memberRegisterVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;
            user = await _userManager.FindByNameAsync(memberRegisterVM.Username);
            if (user is not null)
            {
                ModelState.AddModelError("Username", "Already exist");
                return View();
            }

            user = await _userManager.FindByEmailAsync(memberRegisterVM.Email);
            if (user is not null)
            {
                ModelState.AddModelError("Email", "Already exist");
                return View();
            }
            user = new AppUser
            {
                FullName = memberRegisterVM.Fullname,
                Email = memberRegisterVM.Email,
                UserName = memberRegisterVM.Username,
            };

            var result = await _userManager.CreateAsync(user, memberRegisterVM.Password);
            if (result.Succeeded)
            {
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Login", "Account");
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(memberLoginVM.Username);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, memberLoginVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel paymentVM , string id)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = _context.Users.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            if(user.IsSubscribed == false)
            {
                user.SubscribeTime= DateTime.Now;
                user.SubscriptionEndTime= DateTime.Now.AddDays(30);
                user.IsSubscribed = true;
            }
            else
            {
                    user.SubscribeTime = DateTime.Now;
                    user.SubscriptionEndTime = DateTime.Now.AddDays(30);
            }

            Payment payment = new Payment
            {
                FirstName = paymentVM.FirstName,
                LastName = paymentVM.LastName,
                ExpireMonth = paymentVM.ExpireMonth,
                ExpireYear = paymentVM.ExpireYear,
                CardNumber = paymentVM.CardNumber,
                CCV = paymentVM.CCV
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
