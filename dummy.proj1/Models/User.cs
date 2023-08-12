namespace dummy.Models
{
    public class User : contracts.User
    {
        public ICollection<Post>? Posts { get; } = new List<Post>();
        public ICollection<TodoModel>? TodoModels { get; } = new List<TodoModel>();
    }
}