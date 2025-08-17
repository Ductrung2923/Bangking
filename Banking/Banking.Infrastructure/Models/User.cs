using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public int Role { get; set; }

    public bool? IsActive { get; set; }

    public string? Email { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual ICollection<ExternalTransfer> ExternalTransfers { get; set; } = new List<ExternalTransfer>();

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
