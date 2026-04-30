using CrmContactCenter.Server.Models.Enums;
using System;
using System.Collections.Generic;

namespace CrmContactCenter.Server.Models;

public partial class Account
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int CreatedBy { get; set; }

    public string? AccountNumber { get; set; }

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly? DueDate { get; set; }

    public AccountStatus Status { get; set; } = AccountStatus.pendiente;

    public string Currency { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
}
