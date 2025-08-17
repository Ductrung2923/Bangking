using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class ExternalTransfer
{
    public int ExternalTransferId { get; set; }

    public int? FromAccountId { get; set; }

    public int? ExternalAccountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? StaffId { get; set; }

    public virtual ExternalAccount? ExternalAccount { get; set; }

    public virtual Account? FromAccount { get; set; }

    public virtual User? Staff { get; set; }
}
