using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Digiflex.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(maximumLength:80)]
        public string FullName { get; set; }
        public bool IsSubscribed { get; set; }= false;
        public bool IsDeleted { get; set; } = false;
        public DateTime? SubscribeTime { get; set; }
        public DateTime? SubscriptionEndTime { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        [NotMapped]
        public IFormFile? ProfilePhotoFile { get; set; }
        public List<Comment>? Comments { get; set; }
    }
}
