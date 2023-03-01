namespace Digiflex.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MovieGenre>? MovieGenres { get; set; }
        public List<Slider>? Sliders { get; set; }
    }
}
