using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class TransactionHistoryDTO
    {
        public string TransactionType { get; set; }

        public string FromAccountNumber { get; set; }  
        public string ToAccountNumber { get; set; }    

        public string ExternalAccountNumber { get; set; }
        public string ExternalBankName { get; set; }  

        public decimal Amount { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string StaffName { get; set; }          
    }

}
