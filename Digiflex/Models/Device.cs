using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? IconUrl { get; set; }
        [NotMapped]
        public IFormFile? IconFile { get; set; }
    }
}
