namespace dummy.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string CardExpire { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string Currency { get; set; }
        public string Iban { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}