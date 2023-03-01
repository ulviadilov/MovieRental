using System.ComponentModel.DataAnnotations;

namespace Digiflex.ViewModels
{
    public class MemberLoginViewModel
    {
        [Required]
        [StringLength(maximumLength: 30)]
        public string Username { get; set; }
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 8), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
