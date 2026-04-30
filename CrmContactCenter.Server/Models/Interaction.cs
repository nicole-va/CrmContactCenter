using CrmContactCenter.Server.Models.Enums;
using System;
using System.Collections.Generic;

namespace CrmContactCenter.Server.Models;

public partial class Interaction
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int AgentId { get; set; }

    public int? AccountId { get; set; }

    public InteractionChannel Channel { get; set; }

    public InteractionResult Result { get; set; }

    public string? Notes { get; set; }

    public DateTime InteractionDate { get; set; }

    public int? DurationSeconds { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual User Agent { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
}
