using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class HistoryDTO
    {
        public string TransactionType { get; set; }
        public int? FromAccountID { get; set; } // Thay FromAccountNumber
        public int? ToAccountID { get; set; }   // Thay ToAccountNumber
        public string ExternalAccountNumber { get; set; }
        public int? ExternalBankID { get; set; } // Thay BankName
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int? StaffID { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? SourceTransactionID { get; set; } // Thêm trường mới
        public string Notes { get; set; } // Thêm trường mới
    }
}
