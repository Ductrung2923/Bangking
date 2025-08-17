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
    public class AccountRepository
    {
        private readonly BankingApiContext _context;

        public AccountRepository(BankingApiContext context)
        {
            _context = context;
        }

       
        public async Task<Account?> GetAccountByAccountNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
                
                
        }
        public async Task<bool> UpdateAccountAsync(Account account)
        {
            _context.Accounts.Update(account);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
        }


    }
}
