using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int Role { get; set; } 
        public string Position { get; set; } 
        public string Department { get; set; }
        public string EmployeeCode { get; set; }
        public String DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string? PhotoUrl { get; set; } = null;

        public String HireDate { get; set; }

        public string? Notes { get; set; } = null;

        public DateTime? CreatedAt { get; set; }

    }
}
