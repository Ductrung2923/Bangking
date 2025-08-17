using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? IdentityNumber { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public bool Gender { get; set; }

    public string? PhotoUrl { get; set; }

    public string CardNumber { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
}
