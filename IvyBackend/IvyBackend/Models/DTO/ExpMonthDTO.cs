namespace IvyBackend.Models.DTO
{
    public class ExpMonthDTO
    {
        public int UserId { get; set; }
        public string C_Name { get; set; } = string.Empty;
        public string C_Image { get; set; } = string.Empty;
        public string C_Color { get; set; } = string.Empty;
        public string A_Name { get; set; } = string.Empty;
        public string Cr_Currency { get; set; } = string.Empty;
        public decimal E_Amount { get; set; }
    }
}
