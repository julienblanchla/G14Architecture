using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CardId { get; set; }
        public string CardNumber { get; set; }
        public decimal Balance { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public bool checkBalance()
        {
            return true;
        }
        public bool checkQuota()
        {
            return true;
        }
    }
}
