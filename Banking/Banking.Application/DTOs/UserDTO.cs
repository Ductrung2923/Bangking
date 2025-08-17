using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.DTOs
{
    internal class UserDTO
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public int Role { get; set; }
    }
}
