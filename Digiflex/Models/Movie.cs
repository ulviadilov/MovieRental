using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public int QualityId { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public int? Order { get; set; }
        [StringLength(maximumLength:50)]
        public string Name { get; set; }
        [StringLength(maximumLength:100)]
        public string? PosterImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string DirectorName { get; set; }
        public string? VideoUrl { get; set; }
        public double ImdbScore { get; set; }
        public string Desc { get; set; }
        [StringLength(maximumLength: 250)]
        public string TraillerUrl { get; set; }
        public int VideoLength { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public bool IsDomestic { get; set; }
        [NotMapped]
        public IFormFile? VideoFile { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? ThumbnailFile { get; set; }
        public List<MovieGenre>? MovieGenres { get; set; }
        public Language? Language { get; set; }
        public Quality? Quality { get; set; }
        public Country? Country { get; set; }
        public List<Comment>? Comments { get; set; }


    }
}
