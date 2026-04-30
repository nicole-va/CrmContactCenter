namespace CrmContactCenter.Server.DTOs.Interactions
{
    public class InteractionDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public int? AccountId { get; set; }
        public string Channel { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public DateTime InteractionDate { get; set; }
        public int DurationSeconds { get; set; }
    }
}
