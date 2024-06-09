using Database.Models;

namespace MVC.Models
{
    public class Printers
    {
        public int PrinterId { get; set; }
        public string Name { get; set; }
        public ICollection<Paper> Papers { get; set; }

    }
}
