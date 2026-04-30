namespace CrmContactCenter.Server.DTOs.FollowUps
{
    public class CreateFollowUpDto
    {
        public int CustomerId { get; set; }
        public int AgentId { get; set; }
        public int? InteractionId { get; set; }
        public DateOnly ScheduledDate { get; set; }
        public string? Notes { get; set; }
    }
}
