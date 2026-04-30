namespace CrmContactCenter.Server.DTOs.FollowUps
{
    public class FollowUpDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public int AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public int? InteractionId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? CompletedAt { get; set; }
        public int DaysUntil { get; set; }

    }
}
