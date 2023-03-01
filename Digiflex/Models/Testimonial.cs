using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string? ImageUrl { get; set; }
        public string? AvatarUrl { get; set; }
        public string? LogoUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        [NotMapped]
        public IFormFile? AvatarFile { get; set; }
        [NotMapped]
        public IFormFile? LogoFile { get; set; }
    }
}
