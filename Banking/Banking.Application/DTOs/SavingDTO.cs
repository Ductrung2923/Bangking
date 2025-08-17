using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class SavingDTO
    {
       
            public int CustomerId { get; set; }

        public string AccountNumber { get; set; } = null!;

        public decimal Amount { get; set; }

            public double InterestRate { get; set; }

            public int TermMonths { get; set; }

            public string ReceiveInterestMethod { get; set; } = "ToAccount";

            public int StaffId { get; set; } 
        
    }
}
