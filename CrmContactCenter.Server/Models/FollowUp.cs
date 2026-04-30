using CrmContactCenter.Server.Models.Enums;
using System;
using System.Collections.Generic;

namespace CrmContactCenter.Server.Models;

public partial class FollowUp
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int AgentId { get; set; }

    public int? InteractionId { get; set; }

    public DateOnly ScheduledDate { get; set; }

    public string? Notes { get; set; }

    public FollowUpStatus Status { get; set; } = FollowUpStatus.pendiente;

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Interaction? Interaction { get; set; }
}
