using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; } // "Professor", "Student", "Employee"
        public Card Card { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
