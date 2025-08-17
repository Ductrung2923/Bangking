using Banking.Infrastructure.Data;
using Banking.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class UserRepository
    {
        private readonly Data.BankingApiContext _context;

        public UserRepository(Data.BankingApiContext context)
        {
            _context = context;
        }

        public User? getUserByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.Username == userName);
        }

        public string? GetPasswordByUser(string username)
        {
            var user = getUserByUserName(username);
            return user?.PasswordHash;
        }



        public int AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.UserId;
          
        }

        public void AddStaff(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }

        public string GenerateOTP()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); 
        }

        public string GenPassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public bool UpdatePassword(string username, string newPassword)
        {
            var user = getUserByUserName(username);
            if (user == null) return false;

            user.PasswordHash = newPassword;
            _context.SaveChanges();
            return true;
        }

    }
}
