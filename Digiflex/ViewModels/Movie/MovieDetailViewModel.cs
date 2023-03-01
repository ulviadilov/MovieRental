using Digiflex.Models;

namespace Digiflex.ViewModels
{
    public class MovieDetailViewModel
    {
        public Movie Movie { get; set; }
        public List<Movie> RelatedMovies { get; set; }
        public List<Comment> Comments { get; set; }
        public Comment? Comment { get; set; }
        public string UserId { get; set; }
    }
}
