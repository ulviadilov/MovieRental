using Digiflex.Models;

namespace Digiflex.ViewModels
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<About> Abouts { get; set; }
        public List<Service> Services { get; set; }
        public List<Testimonial> Testimonials { get; set; }
        public List<PaymentPlan> PaymentPlans { get; set; }
        public List<Movie> FeaturedMovies { get; set; }
    }
}
