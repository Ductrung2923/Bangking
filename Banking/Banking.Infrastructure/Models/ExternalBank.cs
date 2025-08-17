using System;
using System.Collections.Generic;

namespace Banking.Infrastructure.Models;

public partial class ExternalBank
{
    public int ExternalBankId { get; set; }

    public string BankName { get; set; } = null!;

    public string? SwiftCode { get; set; }

    public virtual ICollection<ExternalAccount> ExternalAccounts { get; set; } = new List<ExternalAccount>();

    public virtual ICollection<TransactionHistory> TransactionHistories { get; set; } = new List<TransactionHistory>();
}
