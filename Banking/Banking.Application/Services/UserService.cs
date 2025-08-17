using Banking.Application.DTOs;
using Banking.Infrastructure.Models;
using Banking.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Banking.Application.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly OTPService _otpService;
        private readonly EmailService _emailService;

        public UserService(UserRepository userRepository, JwtService jwtService, OTPService otpService, EmailService emailService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _otpService = otpService;
            _emailService = emailService;
        }

        private static readonly Dictionary<string, RegisterDTO> _tempRegisterStorage = new();

        public async Task<string> RegisterStep1Async(RegisterDTO dto)
        {
            var existingUser = _userRepository.getUserByUserName(dto.Username);
            if (existingUser != null) return "Tài khoản đã tồn tại";

            string otp = _userRepository.GenerateOTP();
            _otpService.SaveOtp(dto.Email, otp, expireMinutes: 10);

            _tempRegisterStorage[dto.Email] = dto;

            await _emailService.SendOtpEmailAsync(dto.Email, otp);

            return "Đã lưu OTP thành công và gửi OTP đến email.";
        }



        public async Task<string> RegisterStep2Async(string otp)
        {
            Console.WriteLine($"[DEBUG] OTP nhận được để verify: '{otp}'");

            var email = _otpService.VerifyAndGetEmailByOtp(otp);

            if (email == null)
            {
                Console.WriteLine("[DEBUG] OTP không hợp lệ hoặc hết hạn.");
                return null;
            }

            Console.WriteLine($"[DEBUG] Email tìm thấy theo OTP: {email}");

            if (!_tempRegisterStorage.ContainsKey(email))
            {
                Console.WriteLine("[DEBUG] Email không tồn tại trong temp storage.");
                return null;
            }

            var dto = _tempRegisterStorage[email];



            var user = new User
            {
                Username = dto.Username,
                PasswordHash = dto.Password,
                FullName = dto.FullName,
                Email = dto.Email,
                Role = 0,
                IsActive = false,

            };

            var userId = _userRepository.AddUser(user);

            if (dto.Role == 0)
            {
                DateOnly dateOfBirth = DateOnly.ParseExact(dto.DateOfBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateOnly hireDate = DateOnly.ParseExact(dto.HireDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);


                var staff = new Staff
                {
                    UserId = userId,
                    Position = dto.Position,
                    Department = dto.Department,
                    EmployeeCode = dto.EmployeeCode,
                    DateOfBirth = dateOfBirth,
                    Gender = dto.Gender,
                    PhotoUrl = dto.PhotoUrl,
                    HireDate = hireDate,
                    Notes = dto.Notes,
                    CreatedAt = dto.CreatedAt ?? DateTime.Now
                };
                _userRepository.AddStaff(staff);
            }

            _tempRegisterStorage.Remove(email);
            return _jwtService.GenerateToken(userId, dto.Username, dto.Role);
        }
        

        public async Task<string> ResetPasswordAsync(string username)
        {
            var user = _userRepository.getUserByUserName(username);
            if (user == null)
            {
                return "Tài khoản không tồn tại.";
            }
                
            // Sinh mật khẩu mới
            string newPassword = _userRepository.GenPassword();

            // Gửi mật khẩu mới qua email
            string subject = "Khôi phục mật khẩu";
            string body = $"Mật khẩu mới của bạn là: {newPassword}";
            await _emailService.SendEmailAsync(user.Email, subject, body);

            // Cập nhật mật khẩu mới vào DB
            bool updated = _userRepository.UpdatePassword(username, newPassword);
            return updated ? "Mật khẩu mới đã được gửi tới email của bạn." : "Không thể cập nhật mật khẩu.";
        }


        public string Authenticate(string username, string password)
        {
            var user = _userRepository.getUserByUserName(username);
            if (user == null)
            {
                return null;
            }
            if (user.IsActive == null || user.IsActive == false)
            {
                return "INACTIVE";
            }

            string pass = _userRepository.GetPasswordByUser(username);

            if (password != pass)
            {
                return null;
            }

            return _jwtService.GenerateToken(user.UserId, user.Username, user.Role);
        }
    }
}



