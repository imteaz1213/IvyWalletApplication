using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvyBackend.Models
{
    [Table("Income")]
    public class Income
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; } 
        public int AccountId { get; set; }
        public User? User { get; set; }
        public Category? Category { get; set; } 
        public Account? Account { get; set; }   
    }
}



 