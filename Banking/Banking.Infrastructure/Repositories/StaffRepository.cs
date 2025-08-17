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
    public class StaffRepository
    {
        private readonly BankingApiContext _context;

        public StaffRepository(BankingApiContext context)
        {
            _context = context;
        }

        public async Task<Staff> GetStaffInfoByUserIdAsync(int userId)
        {
            return await _context.Staff
                .Include(s => s.User) 
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<bool> UpdateStaffProfileAsync(Staff staff)
        {
            _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
