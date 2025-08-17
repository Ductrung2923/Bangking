using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class TransactionHistory
{
    public int HistoryId { get; set; }

    public string TransactionType { get; set; } = null!;

    public int? FromAccountId { get; set; }

    public int? ToAccountId { get; set; }

    public string? ExternalAccountNumber { get; set; }

    public int? ExternalBankId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public int? StaffId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? SourceTransactionId { get; set; }

    public string? Notes { get; set; }

    public virtual ExternalBank? ExternalBank { get; set; }

    public virtual Account? FromAccount { get; set; }

    public virtual User? Staff { get; set; }

    public virtual Account? ToAccount { get; set; }
}
