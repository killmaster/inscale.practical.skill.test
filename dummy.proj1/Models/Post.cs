namespace dummy.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int Reactions { get; set; }
        public string[] Tags { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}