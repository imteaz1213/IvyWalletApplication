namespace IvyBackend.Models.DTO
{
    public class BudgetDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<int> CategoryIds { get; set; } = new();
    }
}
