namespace IvyBackend.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
