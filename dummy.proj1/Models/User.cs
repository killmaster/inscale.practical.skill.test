namespace dummy.Models
{
    public class User
    {
        public int Id { get; private set; }
        public ICollection<Post> Posts { get; } = new List<Post>();
        public ICollection<TodoModel> TodoModels { get; } = new List<TodoModel>();
    }
}