namespace dummy.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int Reactions { get; set; }
        public ICollection<string> Tags { get; } = new List<string>();
        public User User { get; set; }
        public int UserId { get; set; }
    }
}