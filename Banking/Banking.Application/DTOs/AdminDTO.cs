using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class AdminDTO
    {
        public int AdminId { get; set; }
        public int? UserId { get; set; }
        public string? PhotoUrl { get; set; }
        public string Phone { get; set; } = null!;
        public bool Gender { get; set; }
        public string Position { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime? CreatedAt { get; set; }

        
        public string? FullName { get; set; }

        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public string Username { get; set; } = null!;
    }
}
