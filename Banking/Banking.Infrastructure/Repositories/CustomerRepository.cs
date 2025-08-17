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
    public class CustomerRepository
    {
        private readonly BankingApiContext _context;

        public CustomerRepository(BankingApiContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllCustomersWithAccountsAsync()
        {
            return await _context.Customers
                .Include(c => c.Accounts)
                .ToListAsync();
        }

        public string GenerateUsername(string fullName)
        {
            var name = fullName?.Trim().Split(' ').LastOrDefault()?.ToLower() ?? "user";
            var random = new Random().Next(100, 999);
            return $"{name}{random}";
        }

        public string GeneratePassword()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> AddCustomerAsync(Customer customer, Account account)
        {
            try
            {
                // Thêm customer vào DB
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(); // Sau khi SaveChanges, CustomerId mới được gán

                // Gán CustomerId cho Account
                account.CustomerId = customer.CustomerId;

                // Thêm account gắn với customer
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm customer: {ex.Message}");
                return false;
            }
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers
                                 .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<List<Customer>> GetNewestCustomersAsync(int count)
        {
            return await _context.Customers
                                 .OrderByDescending(c => c.CreatedAt)
                                 .Take(count)
                                 .ToListAsync();
        }

        ////
        public async Task<Customer?> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers
                                 .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

    }
}
