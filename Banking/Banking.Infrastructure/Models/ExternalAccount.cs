using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class ExternalAccount
{
    public int ExternalAccountId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountHolderName { get; set; } = null!;

    public int? ExternalBankId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ExternalBank? ExternalBank { get; set; }

    public virtual ICollection<ExternalTransfer> ExternalTransfers { get; set; } = new List<ExternalTransfer>();
}
