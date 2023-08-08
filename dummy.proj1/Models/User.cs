namespace dummy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Post> Posts { get; } = new List<Post>();
        public ICollection<TodoModel> TodoModels { get; } = new List<TodoModel>();
    }
}