using System;
using System.Collections.Generic;

namespace CrmContactCenter.Server.Models;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();

    public virtual ICollection<Interaction> Interactions { get; set; } = new List<Interaction>();

    public virtual Role Role { get; set; } = null!;
}
