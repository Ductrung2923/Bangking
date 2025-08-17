using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    public class UpdateProfileDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string FullName { get; set; }

        public string Position { get; set; }

        public string? PhotoUrl { get; set; }

        public string? Notes { get; set; }


    }
}
