namespace IvyBackend.Models.DTO
{
    public class TransactionFilterDTO
    {
        public string Type { get; set; } = string.Empty;
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}
