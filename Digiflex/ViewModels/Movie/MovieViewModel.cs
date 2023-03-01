using Digiflex.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Digiflex.ViewModels
{
    public class MovieViewModel
    {
        public int QualityId { get; set; }
        public int LanguageId { get; set; }
        public int CountryId { get; set; }
        public int? Order { get; set; } = 0;
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        [StringLength(maximumLength: 100)]
        public string? PosterImageUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        [StringLength(maximumLength: 75)]
        public string DirectorName { get; set; }
        public int MyProperty { get; set; }
        public double ImdbScore { get; set; }
        public string Desc { get; set; }
        [StringLength(maximumLength: 250)]
        public string TraillerUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CreateTime { get; set; }
        public int VideoLength { get; set; }
        public bool IsDomestic { get; set; }
        [NotMapped]
        public IFormFile? VideoFile { get; set; }
        [NotMapped]
        public IFormFile? PosterImageFile { get; set; }
        [NotMapped]
        public IFormFile? ThumbnailFile { get; set; }
        public Language? Language { get; set; }
        public Quality? Quality { get; set; }
        public Country? Country { get; set; }
        public List<int>? GenresIds { get; set; }
    }
}
