using Digiflex.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Digiflex.DAL
{
    public class DigiflexContext : IdentityDbContext
    {
        public DigiflexContext(DbContextOptions<DigiflexContext> options): base(options) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Quality> Qualities { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<PaymentPlan> PaymentPlans { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
