using Banking.Infrastructure.Data;
using Banking.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class TransactionHistoryRepository
    {
        private readonly BankingApiContext _context;

        public TransactionHistoryRepository(BankingApiContext context)
        {
            _context = context;
        }


        public async Task<bool> AddHistoryAsync(TransactionHistory history)
        {
            try
            {
                await _context.TransactionHistories.AddAsync(history);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm lịch sử giao dịch: {ex.Message}");
                return false;
            }
        }



        public async Task<List<TransactionHistory>> GetAllWithIncludeAsync()
        {
            return await _context.TransactionHistories
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Staff)
                .OrderByDescending(t => t.CreatedAt)
                .Take(15)
                .ToListAsync();
        }






    }
}
