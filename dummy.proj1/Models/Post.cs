namespace dummy.Models
{
    public class Post
    {
        public int Id { get; private set; }
        public int Reactions { get; private set; }
        public ICollection<Tag> Tags { get; } = new List<Tag>();
        public User User { get; set; }
    }
}