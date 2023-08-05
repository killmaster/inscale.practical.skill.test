namespace dummy.Models
{
    public class TodoModel
    {
        public int Id { get; private set; }
        public string Todo { get; private set; }
        public bool Completed { get; private set; }
        public User User { get; set; }
    }
}