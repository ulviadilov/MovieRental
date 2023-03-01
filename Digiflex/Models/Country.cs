namespace Digiflex.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}
