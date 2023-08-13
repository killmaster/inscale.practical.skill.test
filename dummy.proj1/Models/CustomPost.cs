namespace dummy.Models
{
    public class CustomPost
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserName { get; set; }
        public bool HasFrenchTag { get; set; }
        public bool HasFictionTag { get; set; }
        public bool HasMoreThanTwoReactions { get; set; }
    }
}