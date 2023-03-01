namespace Digiflex.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        public string CommentString { get; set; }
        public AppUser? User { get; set; }
        public Movie? Movie { get; set; }
    }
}
