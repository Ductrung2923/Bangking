using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class ExternalAccountDTO
    {
        public int ExternalBankID { get; set; }              
        public string AccountNumber { get; set; }            
        public string? AccountHolderName { get; set; }
   
        public bool IsFound { get; set; }
    }
}
