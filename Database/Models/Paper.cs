using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models
{
    public class Paper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaperId { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public decimal Value { get; set; }
        public int PrinterId { get; set; }
        [ForeignKey("PrinterId")]
        public Printer Printer { get; set; }
    }
}
