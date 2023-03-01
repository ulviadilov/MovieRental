using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.Controllers
{
    public class MovieController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MovieController(DigiflexContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Movies.Include(x => x.Quality).Include(x => x.MovieGenres).ThenInclude(x => x.Genre).AsQueryable();
            var paginatedMovies = PaginatedList<Movie>.Create(query, 12, page);
            MovieMainViewModel movieMainVM = new MovieMainViewModel
            {
                Movies = paginatedMovies
            };
            return View(movieMainVM);
        }


        public IActionResult MovieDetail(int id )
        {
            string userId =  _userManager.GetUserId(HttpContext.User);
            AppUser user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null) return NotFound();
            if (user.SubscriptionEndTime <= DateTime.Now)
            {
                user.IsSubscribed = false;
                _context.SaveChanges();
                return RedirectToAction("payment","account");
            }
            Movie movie = _context.Movies.Include(x => x.MovieGenres).ThenInclude(x => x.Genre).Include(x => x.Quality).Include(x => x.Comments).Include(x => x.Language).Include(x => x.Country).FirstOrDefault(x => x.Id == id);
            List<Movie> relatedMovies = _context.Movies.Include(x => x.MovieGenres).ThenInclude(x => x.Genre ).Include(x => x.Quality).ToList();
            MovieDetailViewModel movieDetailVM = new MovieDetailViewModel
            {
                Movie = movie,
                RelatedMovies = relatedMovies,
                Comments = _context.Comments.Where(x => x.MovieId == movie.Id).Where(x => x.UserId == user.Id).ToList(),
                UserId= user.Id
            };
            return View(movieDetailVM);
        }

        [HttpPost]
        public IActionResult Comment(Comment comment)
        {
            if(comment is null) return NotFound();
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Index" , "Home");
        }


        public IActionResult Searched(string search)
        {
            List<Movie> movies = _context.Movies.Include(x => x.Quality).Include(x => x.MovieGenres).ThenInclude(x => x.Genre).Where(x => x.Name.Contains(search)).ToList();
            return View(movies);
        }
    }
}
