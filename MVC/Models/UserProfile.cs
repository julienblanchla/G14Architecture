namespace MVC.Models
{
    public class UserProfile
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Card Card { get; set; }
        public string UserType { get; set; }
    }

    public class Card
    {
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int OwnerId { get; set; }
    }
}
