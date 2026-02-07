namespace IvyBackend.Models.DTO
{
    public class BudgetIdDTO
    {
        public string BudgetName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Spent { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal ExpenseAmount { get; set; } 
    }
}
