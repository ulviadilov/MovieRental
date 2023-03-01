using Digiflex.Models;

namespace Digiflex.ViewModels
{
    public class DashboardViewModel
    {
        public List<Movie> Movies { get; set; }
        public List<AppUser> Users { get; set; }
        public List<Payment> Payments { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
