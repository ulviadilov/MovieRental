using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [StringLength(maximumLength:100)]
        public string Desc { get; set; }
        public string? IconImageUrl { get; set; }
        [NotMapped]
        public IFormFile? IconImageFile { get; set; }
    }
}
