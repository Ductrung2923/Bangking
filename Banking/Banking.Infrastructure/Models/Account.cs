using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int? CustomerId { get; set; }

    public string? AccountNumber { get; set; }

    public decimal? Balance { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ExternalTransfer> ExternalTransfers { get; set; } = new List<ExternalTransfer>();

    public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();

    public virtual ICollection<Transaction> TransactionFromAccounts { get; set; } = new List<Transaction>();

    public virtual ICollection<TransactionHistory> TransactionHistoryFromAccounts { get; set; } = new List<TransactionHistory>();

    public virtual ICollection<TransactionHistory> TransactionHistoryToAccounts { get; set; } = new List<TransactionHistory>();

    public virtual ICollection<Transaction> TransactionToAccounts { get; set; } = new List<Transaction>();
}
