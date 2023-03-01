using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int QualityId { get; set; }
        public int GenreId { get; set; }
        public int ReleaseYear { get; set; }
        public double ImdbScore { get; set; }
        public string MovieName { get; set; }
        public string? PosterImageUrl { get; set; }
        public string Desc { get; set; }
        public string TraillerUrl { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        public Quality? Quality { get; set; }
        public Genre? Genre { get; set; }
    }
}
