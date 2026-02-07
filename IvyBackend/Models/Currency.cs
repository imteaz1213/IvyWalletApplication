using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IvyBackend.Models
{
    [Table("Currency")]
    public class Currency
    {
        [Key]
        public int Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
