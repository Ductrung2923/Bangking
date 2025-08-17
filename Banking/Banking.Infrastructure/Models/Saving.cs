using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class Saving
{
    public int SavingId { get; set; }

    public int? CustomerId { get; set; }

    public int? AccountId { get; set; }

    public decimal Amount { get; set; }

    public double InterestRate { get; set; }

    public int TermMonths { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ReceiveInterestMethod { get; set; }

    public decimal? TotalInterestEarned { get; set; }

    public DateTime? LastInterestCalculatedAt { get; set; }

    public string? Status { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Customer? Customer { get; set; }
}
