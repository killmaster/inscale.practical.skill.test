namespace dummy.contracts
{
    public class TodoModel
    {
        public int Id { get; set; }
        public string Todo { get; set; }
        public bool Completed { get; set; }
        public User User { get; set; }
    }
}