using System.Text.Json.Serialization;

namespace MVC.Models
{
    public class UserResponse
    {
        [JsonPropertyName("$id")]
        public string Id { get; set; }
        [JsonPropertyName("$values")]
        public List<User> Values { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Card Card { get; set; }
    }
}
