using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class CustomerDTO
    {
      
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string IdentityNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string PhotoUrl { get; set; }
        public string CardNumber { get; set; }
        public DateTime CreatedAt { get; set; }

  
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAtAc { get; set; }
    }
}
