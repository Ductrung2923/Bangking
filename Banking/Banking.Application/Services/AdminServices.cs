using Banking.Application.DTOs;
using Banking.Infrastructure.Models;
using Banking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class AdminServices
    {
        private readonly AdminRepository _adminRepo;
        private readonly CustomerRepository _customerRepo;
        public AdminServices(AdminRepository adminRepo, CustomerRepository customerRepo)
        {
            _adminRepo = adminRepo;
            _customerRepo = customerRepo;
        }

        public async Task<AdminDTO> GetAdminInfo(int userId)
        {
            var admin = await _adminRepo.GetAdminInfoByUserIdAsync(userId);
            if (admin == null || admin.User == null) return null;

            return new AdminDTO
            {
                AdminId = admin.AdminId,
                UserId = userId,
                Username = admin.User.Username,
                FullName = admin.User.FullName,
                PasswordHash = admin.User.PasswordHash,
                Email = admin.User.Email,
                Phone = admin.Phone,
                Gender = admin.Gender,
                Position = admin.Position,
                PhotoUrl = admin.PhotoUrl,
                Notes = admin.Notes,
                CreatedAt = admin.CreatedAt,
            };
        }

        public async Task<bool> UpdateAdminProfile(int userId, UpdateProfileDTO dto)
        {
            // Lấy admin hiện tại từ DB
            var admin = await _adminRepo.GetAdminInfoByUserIdAsync(userId);
            if (admin == null || admin.User == null) return false;

            // Mapping thủ công
            admin.User.UserId = dto.UserId;
            admin.User.Email = dto.Email;
            admin.User.PasswordHash = dto.PasswordHash;
            admin.User.FullName = dto.FullName;
            admin.Position = dto.Position;
            admin.PhotoUrl = dto.PhotoUrl;
            admin.Notes = dto.Notes;

            return await _adminRepo.UpdateAdminProfileAsync(admin);
        }

        public async Task<List<StaffDTO>> GetAllStaffAccounts()
        {
            var staffList = await _adminRepo.LoadListStaffAsync();

            return staffList.Select(s => new StaffDTO
            {
                UserId = s.UserId,
                StaffId = s.StaffId,
                EmployeeCode = s.EmployeeCode,
                Position = s.Position,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                HireDate = s.HireDate,
                Department = s.Department,
                FullName = s.User?.FullName,
                Email = s.User?.Email,
                IsActive = s.User?.IsActive,
                PhotoUrl = s.PhotoUrl,
                Notes = s.Notes,
                CreatedAt = s.CreatedAt,
            }).ToList();
        }

        public async Task<bool> ApproveStaffAccount(int userId, bool isActive)
        {
            return await _adminRepo.UpdateUserIsActiveAsync(userId, isActive);
        }

        public async Task<List<CustomerDTO>> GetRecentCustomersAsync(int count = 5)
        {
            var customers = await _customerRepo.GetNewestCustomersAsync(count);

            return customers.Select(c => new CustomerDTO
            {
                FullName = c.FullName,
                Email = c.Email,              
                PhotoUrl = c.PhotoUrl,
            }).ToList();
        }

        public async Task<int> GetTotalStaffAsync()
        {
            return await _adminRepo.GetTotalStaffAsync();
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _adminRepo.GetTotalCustomersAsync();
        }

        public async Task<int> GetTotalAccountsAsync()
        {
            return await _adminRepo.GetTotalAccountsAsync();
        }

        public async Task<int> GetTotalExternalBanksAsync()
        {
            return await _adminRepo.GetTotalExternalBanksAsync();
        }

        public async Task<List<WeeklyTransactionStatDTO>> GetWeeklyTransactionStatsAsync()
        {
            var data = await _adminRepo.GetWeeklyTransactionCountsAsync();

            var result = data.Select(d => new WeeklyTransactionStatDTO
            {
                Day = d.Day.ToString(),   // Chuyển DayOfWeek -> string (ex: "Monday")
                Count = d.Count
            }).ToList();

            return result;
        }

        public async Task<decimal> GetTotalCustomerBalanceAsync()
        {
            return await _adminRepo.GetTotalCustomerBalanceAsync();
        }




    }
}
