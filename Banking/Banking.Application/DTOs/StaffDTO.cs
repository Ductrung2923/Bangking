using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class StaffDTO
    {
        public int StaffId { get; set; }
        public int? UserId { get; set; }
        public string Position { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string? PhotoUrl { get; set; }
        public DateOnly HireDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }

        public bool? IsActive { get; set; }
        public string? FullName { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public string Username { get; set; } = null!;
    }
}
