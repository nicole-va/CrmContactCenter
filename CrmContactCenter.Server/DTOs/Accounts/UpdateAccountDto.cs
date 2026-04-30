namespace CrmContactCenter.Server.DTOs.Accounts
{
    public class UpdateAccountDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly? DueDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
