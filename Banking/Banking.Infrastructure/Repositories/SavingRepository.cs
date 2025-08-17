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
    public class SavingRepository
    {
        private readonly BankingApiContext _context;

        public SavingRepository(BankingApiContext context)
        {
            _context = context;
        }

        public async Task<Saving> CreateSavingAsync(Saving saving)
        {
            _context.Savings.Add(saving);
            await _context.SaveChangesAsync();
            return saving;
        }

        public async Task<List<Saving>> GetActiveSavingsAsync()
        {
            return await _context.Savings
                .Include(s => s.Account)
                .Include(s => s.Customer)
                .Where(s => s.Status == "Active")
                .ToListAsync();
        }

        public async Task<bool> UpdateSavingAsync(Saving saving)
        {
            _context.Savings.Update(saving);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Saving?> GetSavingByIdAsync(int savingId)
        {
            return await _context.Savings
                .Include(s => s.Account)
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(s => s.SavingId == savingId);
        }

        public async Task<List<Saving>> GetAllSavingsAsync()
        {
            return await _context.Savings
                .Include(s => s.Account)
                .Include(s => s.Customer)
                .ToListAsync();
        }


        public async Task<List<Saving>> GetMaturedSavingsAsync()
        {
            return await _context.Savings
                .Include(s => s.Customer)
                .Include(s => s.Account)
                .Where(s => s.Status == "Maturity")
                .ToListAsync();
        }


    }
}
