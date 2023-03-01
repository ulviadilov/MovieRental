using Digiflex.DAL;
using Digiflex.Helpers;
using Digiflex.Models;
using Digiflex.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class MovieController : Controller
    {
        private readonly DigiflexContext _context;
        private readonly IWebHostEnvironment _env;

        public MovieController(DigiflexContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(int page = 1)
        {
            var query = _context.Movies.AsQueryable();
            var paginatedMovies = PaginatedList<Movie>.Create(query, 5, page);
            return View(paginatedMovies);
        }


        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Languages = _context.Languages.ToList();
            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            return View();
        }


        [HttpPost]
        public IActionResult Create(MovieViewModel movieVM)
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Languages = _context.Languages.ToList();
            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            if (!ModelState.IsValid) return View();

            if (movieVM.PosterImageFile.ContentType != "image/png" && movieVM.PosterImageFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("PosterImageFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (movieVM.PosterImageFile.Length > 3145728)
            {
                ModelState.AddModelError("PosterImageFile", "You can only upload files under 3mb size");
                return View();
            }

            if (movieVM.ThumbnailFile.ContentType != "image/png" && movieVM.ThumbnailFile.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("ThumbnailFile", "You can only upload png or jpeg format files");
                return View();
            }

            if (movieVM.ThumbnailFile.Length > 3145728)
            {
                ModelState.AddModelError("ThumbnailFile", "You can only upload files under 3mb size");
                return View();
            }

            if (movieVM.VideoFile.ContentType != "video/mp4")
            {
                ModelState.AddModelError("VideoFile" , "You can only upload mp4 format files");
                return View();
            }
            movieVM.PosterImageUrl = movieVM.PosterImageFile.SaveImage(_env.WebRootPath, "uploads/movie");
            movieVM.VideoUrl = movieVM.VideoFile.SaveImage(_env.WebRootPath, "uploads/movievideo");
            movieVM.ThumbnailUrl = movieVM.ThumbnailFile.SaveImage(_env.WebRootPath,"uploads/movie");

            Movie movie = new Movie
            {
                CountryId= movieVM.CountryId,
                Desc= movieVM.Desc,
                DirectorName= movieVM.DirectorName,
                ImdbScore= movieVM.ImdbScore,
                LanguageId= movieVM.LanguageId,
                QualityId= movieVM.QualityId,
                Name= movieVM.Name,
                TraillerUrl= movieVM.TraillerUrl,
                IsDomestic= movieVM.IsDomestic,
                PosterImageUrl= movieVM.PosterImageUrl,
                VideoUrl = movieVM.VideoUrl,
                ReleaseDate = movieVM.ReleaseDate,
                VideoLength= movieVM.VideoLength,
                Order = movieVM.Order,
                 ThumbnailUrl= movieVM.ThumbnailUrl
                
            };
            var genres = _context.Genres.Where(x => movieVM.GenresIds.Contains(x.Id));
            foreach (var item in genres)
            {
                _context.MovieGenres.Add(new MovieGenre { Movie = movie, GenreId = item.Id });
            }

            _context.Movies.Add(movie);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            ViewBag.Languages = _context.Languages.ToList();
            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            ViewBag.Genres = _context.Genres.ToList();
            Movie movie = _context.Movies.Include(x => x.MovieGenres).FirstOrDefault(x => x.Id == id);
            if (movie is null) return NotFound();
            MovieViewModel movieVM = new MovieViewModel
            {
                ReleaseDate = movie.ReleaseDate,
                CountryId = movie.CountryId,
                QualityId = movie.QualityId,
                Desc = movie.Desc,
                DirectorName = movie.DirectorName,
                ImdbScore = movie.ImdbScore,
                LanguageId = movie.LanguageId,
                IsDomestic = movie.IsDomestic,
                Name = movie.Name,
                TraillerUrl = movie.TraillerUrl,
                CreateTime = movie.CreateTime,
                VideoLength= movie.VideoLength,
                Order = movie.Order,
                ThumbnailUrl= movie.ThumbnailUrl,
                PosterImageUrl= movie.PosterImageUrl,
                VideoUrl= movie.VideoUrl,
                GenresIds = new List<int>()
            };

            foreach (var item in movie.MovieGenres)
            {
                movieVM.GenresIds.Add(item.GenreId);
            }
            
            return View(movieVM);
        }


        [HttpPost]
        public IActionResult Update(int id,MovieViewModel movieVM)
        {
            ViewBag.Languages = _context.Languages.ToList();
            ViewBag.Countries = _context.Countries.ToList();
            ViewBag.Qualities = _context.Qualities.ToList();
            ViewBag.Genres = _context.Genres.ToList();

            if (!ModelState.IsValid) return View();
            Movie existMovie = _context.Movies.Include(x => x.MovieGenres).FirstOrDefault(x => x.Id == id);
            if (existMovie is null) return NotFound();
            if (movieVM.PosterImageFile is not null)
            {
                if (movieVM.PosterImageFile.ContentType != "image/png" && movieVM.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (movieVM.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImageFile", "You can only upload files under 3mb size");
                    return View();
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/movie", existMovie.PosterImageUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existMovie.PosterImageUrl = movieVM.PosterImageFile.SaveImage(_env.WebRootPath, "uploads/movie");

            }

            if (movieVM.VideoFile is not null)
            {
                if (movieVM.VideoFile.ContentType != "video/mp4")
                {
                    ModelState.AddModelError("VideoFile", "You can only upload mp4 format files");
                    return View();
                }
                string deletePath = Path.Combine(_env.WebRootPath, "uploads/movievideo", existMovie.VideoUrl);
                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existMovie.VideoUrl = movieVM.VideoFile.SaveImage(_env.WebRootPath , "uploads/movievideo");
            }

            if (movieVM.ThumbnailFile is not null)
            {
                if (movieVM.ThumbnailFile.ContentType != "image/png" && movieVM.ThumbnailFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ThumbnailFile", "You can only upload png or jpeg format files");
                    return View();
                }

                if (movieVM.ThumbnailFile.Length > 3145728)
                {
                    ModelState.AddModelError("ThumbnailFile", "You can only upload files under 3mb size");
                    return View();
                }
                existMovie.ThumbnailUrl = movieVM.ThumbnailFile.SaveImage(_env.WebRootPath , "uploads/movie");
            }
            existMovie.QualityId = movieVM.QualityId;
            existMovie.CountryId = movieVM.CountryId;
            existMovie.LanguageId = movieVM.LanguageId;
            existMovie.Desc = movieVM.Desc;
            existMovie.IsDomestic = movieVM.IsDomestic;
            existMovie.DirectorName = movieVM.DirectorName;
            existMovie.Name = movieVM.Name;
            existMovie.ReleaseDate = movieVM.ReleaseDate;
            existMovie.VideoLength = movieVM.VideoLength;
            existMovie.CreateTime = movieVM.CreateTime;
            existMovie.TraillerUrl = movieVM.TraillerUrl;
            existMovie.ImdbScore = movieVM.ImdbScore;
            existMovie.Order = movieVM.Order;
            existMovie.ThumbnailUrl = movieVM.ThumbnailUrl;
            foreach (var item in existMovie.MovieGenres)
            {
                _context.MovieGenres.Remove(item);
            }
            var genres = _context.Genres.Where(x => movieVM.GenresIds.Contains(x.Id));
            foreach (var item in genres)
            {
                _context.MovieGenres.Add(new MovieGenre { Movie = existMovie, GenreId = item.Id });
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            Movie movie = _context.Movies.Find(id);
            if (movie == null) return NotFound();
            string deletePath = Path.Combine(_env.WebRootPath, "uploads/movie", movie.PosterImageUrl);
            if (System.IO.File.Exists(deletePath))
            {
                System.IO.File.Delete(deletePath);
            }
            string deleteVideoPath = Path.Combine(_env.WebRootPath, "uploads/movie", movie.VideoUrl);
            if (System.IO.File.Exists(deleteVideoPath))
            {
                System.IO.File.Delete(deleteVideoPath);
            }
            string deleteThumbPath = Path.Combine(_env.WebRootPath, "uploads/movie", movie.ThumbnailUrl);
            if (System.IO.File.Exists(deleteThumbPath))
            {
                System.IO.File.Delete(deleteThumbPath);
            }
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return Ok();
        }
    }
}
