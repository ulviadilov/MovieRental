using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class About
    {
        public int Id { get; set; }
        public string MainTitle { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}
