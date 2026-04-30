using CrmContactCenter.Server.Models.Enums;
using System;
using System.Collections.Generic;

namespace CrmContactCenter.Server.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Cedula { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public CustomerStatus Status { get; set; } = CustomerStatus.activo;

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();

    public virtual ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();
}
