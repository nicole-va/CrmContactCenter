namespace CrmContactCenter.Server.DTOs.Interactions
{
    public class CreateInteractionDto
    {
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public int? AccountId { get; set; }
        public string Channel { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public int DurationSeconds { get; set; } = 0;
    }
}
