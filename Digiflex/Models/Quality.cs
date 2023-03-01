namespace Digiflex.Models
{
    public class Quality
    {
        public int Id { get; set; }
        public string QualityName { get; set; }
        public List<Movie>? Movies { get; set; }
        public List<Slider>? Sliders { get; set; }
    }
}
