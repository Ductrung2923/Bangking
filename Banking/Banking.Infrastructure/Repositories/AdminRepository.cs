using Banking.Infrastructure.Data;
using Banking.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Banking.Infrastructure.Repositories
{
    public class AdminRepository
    {
        private readonly BankingApiContext _context;

        public AdminRepository(BankingApiContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetAdminInfoByUserIdAsync(int userId)
        {
            return await _context.Admins
                .Include(a => a.User) 
                .FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task<bool> UpdateAdminProfileAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Staff>> LoadListStaffAsync()
        {
            return await _context.Staff
                .Include(s => s.User) 
                .ToListAsync();
        }

        public async Task<bool> UpdateUserIsActiveAsync(int userId, bool isActive)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.IsActive = isActive;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalStaffAsync()
        {
            return await _context.Staff.CountAsync();
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<int> GetTotalAccountsAsync()
        {
            return await _context.Accounts.CountAsync();
        }

        public async Task<int> GetTotalExternalBanksAsync()
        {
            return await _context.ExternalBanks.CountAsync();
        }

        public async Task<IEnumerable<(DayOfWeek Day, int Count)>> GetWeeklyTransactionCountsAsync()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + 1); 
            var endOfWeek = startOfWeek.AddDays(7); 
            var transactions = await _context.TransactionHistories
                .Where(t => t.CreatedAt.HasValue && t.CreatedAt >= startOfWeek && t.CreatedAt < endOfWeek)
                .ToListAsync(); 
            var grouped = transactions
                .GroupBy(t => t.CreatedAt.Value.DayOfWeek)
                .Select(g => (Day: g.Key, Count: g.Count()))
                .ToList();

            return grouped;
        }

        public async Task<decimal> GetTotalCustomerBalanceAsync()
        {
            return await _context.Accounts
                .SumAsync(a => (decimal?)a.Balance) ?? 0;
        }



    }
}
