using Banking.Infrastructure.Data;
using Banking.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace Banking.Infrastructure.Repositories
{
    public class TransactionRepository
    {
        private readonly BankingApiContext _context;

        public TransactionRepository(BankingApiContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTransactionAsync(Transaction transaction)
        {
            try
            {
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm giao dịch: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddExternalTransferAsync(ExternalTransfer transfer)
        {
            try
            {
                _context.ExternalTransfers.Add(transfer);
                await _context.SaveChangesAsync();
                return true;
            }catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm giao dịch: {ex.Message}");
                return false;
            }
           
        }



    }
}
