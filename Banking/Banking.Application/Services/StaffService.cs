using Banking.Application.DTOs;
using Banking.Infrastructure.Models;
using Banking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Banking.Application.DTOs.SavingDTO;

namespace Banking.Application.Services
{
    public class StaffService
    {
        private readonly StaffRepository _stafRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly EmailService _emailService;
        private readonly AccountRepository _accountRepo;
        private readonly TransactionHistoryRepository _transactionHistoryRepo;
        private readonly TransactionRepository _transactionRepo;
        private readonly ExtraAccountRepository _extraAccountRepo;
        private readonly SavingRepository _savingRepo;

        public StaffService(StaffRepository stafRepo, CustomerRepository customerRepo, EmailService emailService, AccountRepository accountRepo, TransactionHistoryRepository transactionHistoryRepo, TransactionRepository transactionRepo, ExtraAccountRepository extraAccountRepo, SavingRepository savingRepo)
        {
            _stafRepo = stafRepo;
            _customerRepo = customerRepo;
            _emailService = emailService;
            _accountRepo = accountRepo;
            _transactionHistoryRepo = transactionHistoryRepo;
            _transactionRepo = transactionRepo;
            _extraAccountRepo = extraAccountRepo;
            _savingRepo = savingRepo;
        }

        public async Task<StaffDTO> GetStaffInfo(int userId)
        {
            var staff = await _stafRepo.GetStaffInfoByUserIdAsync(userId);
            if(staff == null || staff.User == null)
            {
                return null;
            }
            return new StaffDTO
            {
                UserId = userId,
                StaffId = staff.StaffId,
                Username = staff.User.Username,
                PasswordHash = staff.User.PasswordHash,
                Email = staff.User.Email,
                FullName = staff.User.FullName,
                EmployeeCode = staff.EmployeeCode,
                Position = staff.Position,
                Department = staff.Department,
                DateOfBirth = staff.DateOfBirth,
                Gender = staff.Gender,
                PhotoUrl = staff.PhotoUrl,
                HireDate = staff.HireDate,
                Notes = staff.Notes,
                CreatedAt = staff.CreatedAt,
                
            };
            
        }

        public async Task<bool> UpdateStaffProfile(int userId, UpdateProfileDTO dto)
        {

            var staff = await _stafRepo.GetStaffInfoByUserIdAsync(userId);
            if (staff == null || staff.User == null) return false;

            
            staff.User.UserId = dto.UserId;
            staff.User.Email = dto.Email;
            staff.User.PasswordHash = dto.PasswordHash;
            staff.User.FullName = dto.FullName;
            staff.Position = dto.Position;
            staff.PhotoUrl = dto.PhotoUrl;
            staff.Notes = dto.Notes;

            return await _stafRepo.UpdateStaffProfileAsync(staff);
        }


        public async Task<List<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _customerRepo.GetAllCustomersWithAccountsAsync();

            var result = new List<CustomerDTO>();

            foreach (var customer in customers)
            {
                foreach (var account in customer.Accounts)
                {
                    result.Add(new CustomerDTO
                    {
                       
                        FullName = customer.FullName ?? "",
                        Email = customer.Email ?? "",
                        Phone = customer.Phone ?? "",
                        Address = customer.Address ?? "",
                        IdentityNumber = customer.IdentityNumber ?? "",
                        DateOfBirth = customer.DateOfBirth,
                        Gender = customer.Gender,
                        PhotoUrl = customer.PhotoUrl ?? "",
                        CardNumber = customer.CardNumber,
                        CreatedAt = customer.CreatedAt ?? DateTime.MinValue,

                        
                        AccountNumber = account.AccountNumber ?? "",
                        Balance = account.Balance ?? 0,
                        CreatedAtAc = account.CreatedAt ?? DateTime.MinValue
                    });
                }
            }

            return result;
        }

        public async Task<bool> AddCustomerAsync(CustomerDTO dto)
        {
            var existingCustomer = await _customerRepo.GetCustomerByEmailAsync(dto.Email);
            if (existingCustomer != null)
            {
                return false; // Email đã tồn tại
            }

            string username = _customerRepo.GenerateUsername(dto.FullName);
            string password = _customerRepo.GeneratePassword();

            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                Address = dto.Address,
                IdentityNumber = dto.IdentityNumber,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhotoUrl = dto.PhotoUrl,
                CardNumber = dto.CardNumber,
                CreatedAt = DateTime.Now
            };

           
            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                Balance = dto.Balance,
                CreatedAt = DateTime.Now
            };

            
            bool result = await _customerRepo.AddCustomerAsync(customer, account);

            // Gửi mail nếu lưu thành công
            if (result)
            {
                string subject = "Tạo tài khoản ngân hàng thành công";
                string message = $@"
            Xin chào <b>{dto.FullName}</b>,<br/><br/>
            Bạn đã được đăng ký thành công tài khoản ngân hàng.<br/><br/>
            <b>Account Number:</b> {dto.AccountNumber}<br/>
            <b>Tên đăng nhập (tham khảo):</b> {username}<br/>
            <b>Mật khẩu (tham khảo):</b> {password}<br/><br/>
            Vui lòng không trả lời email này.";

                await _emailService.SendEmailAsync(dto.Email, subject, message);
            }

            return result;
        }



        public async Task<AccountDTO?> CheckAccountAsync(string accountNumber)
        {
            var account = await _accountRepo.GetAccountByAccountNumberAsync(accountNumber);
            if (account == null || account.Customer == null)
                return null;

            return new AccountDTO
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance ?? 0,
                FullName = account.Customer.FullName,
                Email = account.Customer.Email,
                Phone = account.Customer.Phone,
                IdentityNumber = account.Customer.IdentityNumber,
                CustomerID = account.CustomerId
            };
        }

        public async Task<string?> TransferInternalAsync(TransferinternalDTO dto)
        {
            var fromAccount = await _accountRepo.GetAccountByAccountNumberAsync(dto.FromAccountNumber);
            var toAccount = await _accountRepo.GetAccountByAccountNumberAsync(dto.ToAccountNumber);

            if (fromAccount == null)
                return "Tài khoản gửi không tồn tại.";

            if (toAccount == null)
                return "Tài khoản nhận không tồn tại.";

            decimal currentBalance = fromAccount.Balance ?? 0;
            decimal minimumBalance = 50000;

            if (currentBalance < dto.Amount)
                return "Số dư không đủ để chuyển.";

            if ((currentBalance - dto.Amount) < minimumBalance)
                return $"Tài khoản phải giữ lại tối thiểu {minimumBalance:N0} VND sau khi chuyển.";

            fromAccount.Balance -= dto.Amount;
            toAccount.Balance += dto.Amount;

            await _accountRepo.UpdateAccountAsync(fromAccount);
            await _accountRepo.UpdateAccountAsync(toAccount);

            var transaction = new Transaction
            {
                FromAccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = dto.Amount,
                Type = "Transfer",
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now
            };

            var successTransaction = await _transactionRepo.AddTransactionAsync(transaction);
            if (!successTransaction)
                return "Lỗi khi tạo giao dịch.";

            var transactionHistory = new TransactionHistory
            {
                TransactionType = "Transfer",
                FromAccountId = fromAccount.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now,
                SourceTransactionId = transaction.TransactionId
            };

            var successHistory = await _transactionHistoryRepo.AddHistoryAsync(transactionHistory);
            if (!successHistory)
                return "Lỗi khi tạo lịch sử giao dịch.";

            if (fromAccount.Customer != null)
            {
                string subject = "Xác nhận chuyển tiền thành công";
                string message = $@"
Xin chào <b>{fromAccount.Customer.FullName}</b>,<br/><br/>
Bạn đã chuyển thành công <b>{dto.Amount:N0} VND</b> đến tài khoản <b>{toAccount.AccountNumber}</b><br/>
Tài khoản thụ hưởng: <b>{toAccount.Customer?.FullName}</b>.<br/><br/>
Cảm ơn bạn đã sử dụng dịch vụ ngân hàng.";

                await _emailService.SendEmailAsync(fromAccount.Customer.Email, subject, message);
            }

            return null; // ✅ Thành công
        }


        public List<ExternalBank> GetAllExternalBanks()
        {
            return _extraAccountRepo.GetAllExternalBanks();
        }

        public ExternalAccountDTO ExternalAccount(int bankId, string accountNumber)
        {
            var externalAccount = _extraAccountRepo.GetExternalAccountByBankAndNumber(bankId, accountNumber);

            if (externalAccount == null)
            {
                return new ExternalAccountDTO
                {
                    AccountNumber = accountNumber,
                    ExternalBankID = bankId,
                    IsFound = false
                };
            }

            return new ExternalAccountDTO
            {
                AccountNumber = externalAccount.AccountNumber,
                ExternalBankID = externalAccount.ExternalAccountId,
                AccountHolderName = externalAccount.AccountHolderName,
                IsFound = true
            };
        }


        public async Task<string?> TransferExternalAsync(TransferExternalDTO dto)
        {
            // 1. Lấy tài khoản nguồn
            var fromAccount = await _accountRepo.GetAccountByAccountNumberAsync(dto.FromAccountNumber);
            if (fromAccount == null)
                return "Tài khoản gửi không tồn tại.";

            decimal currentBalance = fromAccount.Balance ?? 0;
            decimal minimumBalance = 50000;

           
            if (currentBalance < dto.Amount)
                return "Số dư không đủ để chuyển.";

            // 3. Phải để lại ít nhất 50.000 VND trong tài khoản
            if ((currentBalance - dto.Amount) < minimumBalance)
                return $"Tài khoản phải giữ lại tối thiểu {minimumBalance:N0} VND sau khi chuyển.";

            // 4. Lấy tài khoản nhận ngoài hệ thống
            var externalAccount = _extraAccountRepo.GetExternalAccountByBankAndNumber(dto.ExternalBankID, dto.ToAccountNumber);
            if (externalAccount == null)
                return "Tài khoản nhận tại ngân hàng ngoài không tồn tại.";

            // 5. Trừ tiền
            fromAccount.Balance -= dto.Amount;
            await _accountRepo.UpdateAccountAsync(fromAccount);

            // 6. Ghi vào bảng ExternalTransfers
            var externalTransfer = new ExternalTransfer
            {
                FromAccountId = fromAccount.AccountId,
                ExternalAccountId = externalAccount.ExternalAccountId,
                Amount = dto.Amount,
                TransactionDate = DateTime.Now,
                Status = "Success",
                CreatedAt = DateTime.Now,
                StaffId = dto.StaffId
            };

            var successTransfer = await _transactionRepo.AddExternalTransferAsync(externalTransfer);
            if (!successTransfer)
                return "Lỗi khi tạo bản ghi giao dịch chuyển ngoài.";

            // 7. Ghi vào TransactionHistory
            var transactionHistory = new TransactionHistory
            {
                TransactionType = "ExternalTransfer",
                FromAccountId = fromAccount.AccountId,
                ExternalAccountNumber = externalAccount.AccountNumber,
                ExternalBankId = externalAccount.ExternalBankId,
                Amount = dto.Amount,
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now,
                SourceTransactionId = externalTransfer.ExternalTransferId
            };

            var successHistory = await _transactionHistoryRepo.AddHistoryAsync(transactionHistory);
            if (!successHistory)
                return "Lỗi khi tạo lịch sử giao dịch.";

            // 8. Gửi email xác nhận
            if (fromAccount.Customer != null)
            {
                string subject = "Xác nhận chuyển tiền ra ngân hàng ngoài";
                string message = $@"
Xin chào <b>{fromAccount.Customer.FullName}</b>,<br/><br/>
Bạn đã chuyển thành công <b>{dto.Amount:N0} VND</b> đến tài khoản <b>{externalAccount.AccountNumber}</b> tại ngân hàng <b>{externalAccount.ExternalBank.BankName}</b><br/>
Chủ tài khoản thụ hưởng: <b>{externalAccount.AccountHolderName}</b><br/><br/>
Cảm ơn bạn đã sử dụng dịch vụ ngân hàng.";

                await _emailService.SendEmailAsync(fromAccount.Customer.Email, subject, message);
            }

            return null; 
        }



        public async Task<string?> DepositAsync(DepositWithdrawDTO dto)
        {
            var account = await _accountRepo.GetAccountByAccountNumberAsync(dto.AccountNumber);
            if (account == null)
                return "Tài khoản không tồn tại.";
            if (dto.Amount <= 0)
                return "Số tiền nạp không hợp lệ.";

            account.Balance += dto.Amount;
            await _accountRepo.UpdateAccountAsync(account);

            var transaction = new Transaction
            {
                FromAccountId = account.AccountId,
                ToAccountId = null,
                Amount = dto.Amount,
                Type = "Deposit",
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now
            };

            var successTransaction = await _transactionRepo.AddTransactionAsync(transaction);
            if (!successTransaction) return "Không tạo được giao dịch.";

            var history = new TransactionHistory
            {
                TransactionType = "Deposit",
                FromAccountId = account.AccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now,
                SourceTransactionId = transaction.TransactionId
            };

            var successHistory = await _transactionHistoryRepo.AddHistoryAsync(history);
            if (!successHistory) return "Không tạo được lịch sử giao dịch.";

            if (account.Customer != null)
            {
                string subject = "Xác nhận nạp tiền";
                string message = $@"
Xin chào <b>{account.Customer.FullName}</b>,<br/><br/>
Bạn đã nạp thành công <b>{dto.Amount:N0} VND</b> vào tài khoản <b>{account.AccountNumber}</b>.<br/>
Số dư hiện tại: <b>{account.Balance:N0} VND</b><br/><br/>
Cảm ơn bạn đã sử dụng dịch vụ ngân hàng.";

                await _emailService.SendEmailAsync(account.Customer.Email, subject, message);
            }

            return null; // ✅ tất cả thành công
        }


        public async Task<string?> WithdrawAsync(DepositWithdrawDTO dto)
        {
            var account = await _accountRepo.GetAccountByAccountNumberAsync(dto.AccountNumber);
            if (account == null) return "Tài khoản không tồn tại.";

            if (dto.Amount <= 0) return "Số tiền rút không hợp lệ.";

            decimal currentBalance = account.Balance ?? 0;
            decimal minimumBalance = 50000;

            if (currentBalance < dto.Amount)
                return "Số dư không đủ.";

            if (currentBalance - dto.Amount < minimumBalance)
                return $"Tài khoản phải giữ lại tối thiểu {minimumBalance:N0} VND sau khi rút.";

            // ✅ Trừ tiền
            account.Balance -= dto.Amount;
            await _accountRepo.UpdateAccountAsync(account);

            // ✅ Giao dịch
            var transaction = new Transaction
            {
                FromAccountId = account.AccountId,
                Amount = dto.Amount,
                Type = "Withdraw",
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now
            };
            await _transactionRepo.AddTransactionAsync(transaction);

            // ✅ Lịch sử giao dịch
            var history = new TransactionHistory
            {
                TransactionType = "Withdraw",
                FromAccountId = account.AccountId,
                Amount = dto.Amount,
                Description = dto.Description,
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now,
                SourceTransactionId = transaction.TransactionId
            };
            await _transactionHistoryRepo.AddHistoryAsync(history);

            // ✅ Gửi email
            if (account.Customer != null)
            {
                string subject = "Xác nhận rút tiền";
                string message = $@"
Xin chào <b>{account.Customer.FullName}</b>,<br/><br/>
Bạn đã rút <b>{dto.Amount:N0} VND</b> từ tài khoản <b>{account.AccountNumber}</b>.<br/>
Số dư còn lại: <b>{account.Balance:N0} VND</b><br/><br/>
Cảm ơn bạn đã sử dụng dịch vụ ngân hàng.";

                await _emailService.SendEmailAsync(account.Customer.Email, subject, message);
            }

            return null; // ✅ Không có lỗi → thành công
        }


        public async Task<string?> CreateSavingAsync(SavingDTO dto)
        {
            var account = await _accountRepo.GetAccountByAccountNumberAsync(dto.AccountNumber);
            if (account == null)
                return "Tài khoản không tồn tại.";

            if (account.Balance < dto.Amount)
                return "Số dư không đủ để gửi tiết kiệm.";

            decimal minimumBalance = 50000;
            if ((account.Balance - dto.Amount) < minimumBalance)
                return $"Tài khoản phải giữ lại tối thiểu {minimumBalance:N0} VND sau khi gửi.";

            // ✅ Trừ tiền từ tài khoản
            account.Balance -= dto.Amount;
            await _accountRepo.UpdateAccountAsync(account);

            // ✅ Tạo sổ tiết kiệm
            var saving = new Saving
            {
                CustomerId = dto.CustomerId,
                AccountId = account.AccountId,
                Amount = dto.Amount,
                InterestRate = dto.InterestRate,
                TermMonths = dto.TermMonths,
                ReceiveInterestMethod = dto.ReceiveInterestMethod,
                Status = "Active",
                CreatedAt = DateTime.Now,
                StartDate = DateTime.Now
            };

            var result = await _savingRepo.CreateSavingAsync(saving);
            if (result == null)
                return "Lỗi khi tạo sổ tiết kiệm.";

            // ✅ Ghi lịch sử giao dịch
            var history = new TransactionHistory
            {
                TransactionType = "SavingDeposit",
                FromAccountId = account.AccountId,
                Amount = dto.Amount,
                Description = $"Gửi tiết kiệm kỳ hạn {dto.TermMonths} tháng",
                Status = "Success",
                StaffId = dto.StaffId,
                CreatedAt = DateTime.Now
            };

            await _transactionHistoryRepo.AddHistoryAsync(history);

            // ✅ Gửi email cho khách hàng
            if (account.Customer != null)
            {
                string subject = "Xác nhận gửi tiết kiệm thành công";
                string message = $@"
Xin chào <b>{account.Customer.FullName}</b>,<br/><br/>
Bạn đã gửi tiết kiệm thành công số tiền <b>{dto.Amount:N0} VND</b> từ tài khoản <b>{account.AccountNumber}</b>.<br/>
<b>Kỳ hạn:</b> {dto.TermMonths} tháng<br/>
<b>Lãi suất:</b> {dto.InterestRate}%/năm<br/>
<b>Hình thức nhận lãi:</b> {(dto.ReceiveInterestMethod == "AddToPrincipal" ? "Gộp vào gốc" : "Chuyển vào tài khoản")}.<br/><br/>
Cảm ơn bạn đã sử dụng dịch vụ ngân hàng.";

                await _emailService.SendEmailAsync(account.Customer.Email, subject, message);
            }

            return null; // ✅ Thành công
        }

        public async Task<string> CalculateSavingsInterestAsync()
        {
            var savings = await _savingRepo.GetActiveSavingsAsync();
            int updated = 0;

            foreach (var saving in savings)
            {
                if (saving.LastInterestCalculatedAt == null)
                    saving.LastInterestCalculatedAt = saving.StartDate;

                var monthsPassed = (int)((DateTime.Now.Date - saving.LastInterestCalculatedAt.Value.Date).TotalDays / 30);
                if (monthsPassed <= 0)
                    continue;

                decimal interestPerMonth = saving.Amount * ((decimal)saving.InterestRate / 100) / 12;
                decimal interestToAdd = interestPerMonth * monthsPassed;

               
                if (saving.ReceiveInterestMethod == "ToAccount")
                {
                    var account = await _accountRepo.GetAccountByIdAsync(saving.AccountId.Value);
                    if (account != null )
                    {
                        account.Balance += interestToAdd;
                        await _accountRepo.UpdateAccountAsync(account);
                    }
                    saving.TotalInterestEarned += interestToAdd;
                    if (saving.CustomerId.HasValue)
                    {
                        var customer = await _customerRepo.GetCustomerByIdAsync(saving.CustomerId.Value);
                        if (customer != null && !string.IsNullOrEmpty(customer.Email))
                        {
                            string subject = "Thông báo nhận lãi định kỳ";
                            string body = $"Kính chào {customer.FullName},\n\n" +
                                          $"Sổ tiết kiệm của bạn đã đến kỳ nhận lãi vào ngày {DateTime.Now:dd/MM/yyyy}.\n" +
                                          $"Số tiền lãi {interestToAdd:N0} VND đã được chuyển vào tài khoản {account.AccountNumber}.\n\n" +
                                          $"Trân trọng,\nNgân hàng Banking";
                            
                            await _emailService.SendEmailAsync(customer.Email, subject, body);
                        }
                     }
                    }
                else if (saving.ReceiveInterestMethod == "AtMaturity")
                {
                    
                    saving.TotalInterestEarned += interestToAdd;

                    
                    saving.Status = "Maturity";
                    if (saving.CustomerId.HasValue)
                    {
                        var customer = await _customerRepo.GetCustomerByIdAsync(saving.CustomerId.Value);
                        if (customer != null && !string.IsNullOrEmpty(customer.Email))
                        {
                            string subject = "Thông báo đáo hạn sổ tiết kiệm";
                            string body = $"Kính chào {customer.FullName},\n\n" +
                                          $"Sổ tiết kiệm của bạn đã đến kỳ đáo hạn vào ngày {DateTime.Now:dd/MM/yyyy}.\n" +
                                          $"Tổng tiền lãi tích lũy: {saving.TotalInterestEarned:N0} VND.\n" +
                                          $"Vui lòng đến ngân hàng để rút tiền.\n\n" +
                                          $"Trân trọng,\nNgân hàng Banking";

                            await _emailService.SendEmailAsync(customer.Email, subject, body);
                        }
                    }


                }

                saving.LastInterestCalculatedAt = saving.LastInterestCalculatedAt.Value.AddMonths(monthsPassed);


                updated++;
                await _savingRepo.UpdateSavingAsync(saving);
            }

            return $"{updated} savings updated with interest.";
        }


        public async Task<List<ViewSavingDTO>> GetAllSavingsAsync()
        {
            var savings = await _savingRepo.GetAllSavingsAsync();

            return savings.Select(s => new ViewSavingDTO
            {
                SavingID = s.SavingId,
                CustomerID = s.CustomerId ?? 0,
                CustomerName = s.Customer?.FullName ?? "N/A",
                PhotoUrl = s.Customer.PhotoUrl,
                AccountID = s.AccountId ?? 0,
                AccountNumber = s.Account?.AccountNumber ?? "N/A",
                Amount = s.Amount,
                InterestRate = (float)s.InterestRate, // ép từ double -> float
                TermMonths = s.TermMonths,
                StartDate = s.StartDate ?? DateTime.MinValue,
                EndDate = (s.StartDate ?? DateTime.MinValue).AddMonths(s.TermMonths),
                ReceiveInterestMethod = s.ReceiveInterestMethod ?? "N/A",
                Status = s.Status ?? "Unknown",
                TotalInterestEarned = s.TotalInterestEarned ?? 0
            }).ToList();
        }

        public async Task<List<TransactionHistoryDTO>> GetFormattedHistoriesAsync()
        {
            var entities = await _transactionHistoryRepo.GetAllWithIncludeAsync();

            return entities.Select(t => new TransactionHistoryDTO
            {
                TransactionType = t.TransactionType,
                FromAccountNumber = t.FromAccount?.AccountNumber ?? "N/A",
                ToAccountNumber = t.ToAccount?.AccountNumber ?? "N/A",
                ExternalAccountNumber = t.ExternalAccountNumber ?? "N?A",
                Amount = t.Amount,
                Status = t.Status,
                CreatedAt = t.CreatedAt ?? DateTime.MinValue
            }).ToList();
        }

        public async Task<List<ViewSavingWithWithdrawnDTO>> GetMaturedSavingsWithWithdrawnAsync()
        {
            var matured = await _savingRepo.GetMaturedSavingsAsync();

            return matured.Select(s => new ViewSavingWithWithdrawnDTO
            {
                SavingID = s.SavingId,
                CustomerID = s.CustomerId ?? 0,
                CustomerName = s.Customer?.FullName ?? "N/A",
                PhotoUrl = s.Customer?.PhotoUrl,
                AccountID = s.AccountId ?? 0,
                AccountNumber = s.Account?.AccountNumber ?? "N/A",
                Amount = s.Amount,
                InterestRate = (float)s.InterestRate,
                TermMonths = s.TermMonths,
                StartDate = s.StartDate ?? DateTime.MinValue,
                EndDate = (s.StartDate ?? DateTime.MinValue).AddMonths(s.TermMonths),
                ReceiveInterestMethod = s.ReceiveInterestMethod ?? "N/A",
                Status = s.Status ?? "Unknown",
                TotalInterestEarned = s.TotalInterestEarned ?? 0,
                Withdrawn = false  // mặc định false, bạn có thể đổi theo logic
            }).ToList();
        }

        



    }
}
