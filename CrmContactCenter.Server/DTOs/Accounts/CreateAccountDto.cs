namespace CrmContactCenter.Server.DTOs.Accounts
{
    public class CreateAccountDto
    {
        public int CustomerId { get; set; }
        public int CreatedBy { get; set; }
        public string? AccountNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly? DueDate { get; set; }
        public string Currency { get; set; } = "USD";
        public string? Notes { get; set; }
    }
}
