using System.ComponentModel.DataAnnotations;

namespace Digiflex.ViewModels
{
    public class PaymentViewModel
    {
        [Required]
        [StringLength(maximumLength: 30)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(maximumLength: 30)]
        public string LastName { get; set; }
        [Required]
        [StringLength(maximumLength: 16)]
        public string CardNumber { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public int CCV { get; set; }
        [Required]
        public int ExpireYear { get; set; }
        [Required]
        public int ExpireMonth { get; set; }

    }
}
