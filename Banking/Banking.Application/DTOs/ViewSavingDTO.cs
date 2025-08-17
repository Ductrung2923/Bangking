using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class ViewSavingDTO
    {
        public int SavingID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string PhotoUrl { get; set; }
        public int AccountID { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public float InterestRate { get; set; }
        public int TermMonths { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ReceiveInterestMethod { get; set; }
        public string Status { get; set; }
        public decimal TotalInterestEarned { get; set; }
    }

}
