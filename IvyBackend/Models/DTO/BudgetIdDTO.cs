namespace IvyBackend.Models.DTO
{
    public class BudgetIdDTO
    {
        public int Id { get; set; }   
        public string BudgetName { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal Spent { get; set; }
        public string Color { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;    
        public int CategoryId { get; set; } 
        public decimal ExpenseAmount { get; set; } 
    }
}
