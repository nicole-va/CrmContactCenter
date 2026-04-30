namespace CrmContactCenter.Server.DTOs.Accounts
{
    public class AccountDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? AccountNumber { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
