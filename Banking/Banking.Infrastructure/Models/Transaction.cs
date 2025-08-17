using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? FromAccountId { get; set; }

    public int? ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public int? StaffId { get; set; }

    public virtual Account? FromAccount { get; set; }

    public virtual User? Staff { get; set; }

    public virtual Account? ToAccount { get; set; }
}
