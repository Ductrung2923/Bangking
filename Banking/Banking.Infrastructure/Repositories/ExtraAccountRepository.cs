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
    public class ExtraAccountRepository
    {
        private readonly BankingApiContext _context;

        public ExtraAccountRepository(BankingApiContext context)
        {
            _context = context;
        }

        public List<ExternalBank> GetAllExternalBanks()
        {
            return _context.ExternalBanks
                .OrderBy(b => b.BankName)
                .ToList();
        }
        public ExternalAccount? GetExternalAccountByBankAndNumber(int bankId, string accountNumber)
        {
            return _context.ExternalAccounts
                .Include(ea => ea.ExternalBank)
                .FirstOrDefault(ea => ea.ExternalBankId == bankId && ea.AccountNumber == accountNumber);
        }
    }
}
