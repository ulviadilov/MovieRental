using Digiflex.Models;
using Microsoft.AspNetCore.Identity;

namespace Digiflex.Areas.Manage.Services
{
    public class AdminLayoutService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminLayoutService(UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        public async Task<AppUser> GetUser()
        {
            string name = _httpContextAccessor.HttpContext.User.Identity.Name;
            AppUser user = await _userManager.FindByNameAsync(name);
            return user;
        }
    }
}
