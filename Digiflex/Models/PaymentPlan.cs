using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class PaymentPlan
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? IconUrl { get; set; }
        public double Price { get; set; }
        public string PlanTitle { get; set; }
        [NotMapped]
        public IFormFile IconFile { get; set; }
        public string? BackgroudVideoUrl { get; set; }
        [NotMapped]
        public IFormFile? BackgroundVideoFile { get; set; }
    }
}
