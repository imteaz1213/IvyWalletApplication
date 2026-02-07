using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvyBackend.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Income> Incomes { get; set; } = new List<Income>();
        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
