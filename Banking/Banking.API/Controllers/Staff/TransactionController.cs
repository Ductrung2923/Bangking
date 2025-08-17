using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        
        private readonly StaffService _staffService;

        public TransactionController(StaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet("check-account/{accountNumber}")]
        public async Task<IActionResult> CheckAccount(string accountNumber)
        {
            var account = await _staffService.CheckAccountAsync(accountNumber);
            if (account == null)
            {
                return NotFound(new { message = "Số tài khoản không tồn tại." });
            }

            return Ok(account);
        }

        [HttpPost("transfer-internal")]
        public async Task<IActionResult> TransferInternal([FromBody] TransferinternalDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var error = await _staffService.TransferInternalAsync(dto);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { message = "Chuyển tiền thành công." });
        }


        [HttpGet("external-banks")]
        public IActionResult GetAllExternalBanks()
        {
            var banks = _staffService.GetAllExternalBanks();
            return Ok(banks);
        }


        [HttpGet("check-external-account")]
        public IActionResult CheckExternalAccount([FromQuery] int bankId, [FromQuery] string accountNumber)
        {
            var result = _staffService.ExternalAccount(bankId, accountNumber);

            if (!result.IsFound)
            {
                return NotFound(new { message = "Không tìm thấy tài khoản ngoài tương ứng." });
            }

            return Ok(result);
        }

        [HttpPost("transfer-external")]
        public async Task<IActionResult> TransferExternal([FromBody] TransferExternalDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var error = await _staffService.TransferExternalAsync(dto);

            if (error != null)
            {
                
                return BadRequest(new { message = error });
            }

            // ✅ Thành công
            return Ok(new { message = "Chuyển tiền ra ngân hàng ngoài thành công." });
        }



        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositWithdrawDTO dto)
        {
            var result = await _staffService.DepositAsync(dto);

            if (result != null)
            {
                return BadRequest(new { message = result }); // ❌ lỗi
            }

            return Ok(new { message = "Nạp tiền thành công." }); // ✅ trả về rõ ràng
        }


        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] DepositWithdrawDTO dto)
        {
            var errorMessage = await _staffService.WithdrawAsync(dto);

            if (string.IsNullOrEmpty(errorMessage))
                return Ok(new { message = "Rút tiền thành công" });


            // ❗ Trả lại lỗi đúng lý do
            return BadRequest(new { message = errorMessage });
        }

        [HttpPost("saving")]
        public async Task<IActionResult> CreateSaving([FromBody] SavingDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var error = await _staffService.CreateSavingAsync(dto);

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { message = "Tạo sổ tiết kiệm thành công." });
        }

        [HttpPost("calculate-interest")]
        public async Task<IActionResult> CalculateInterest()
        {
            var error = await _staffService.CalculateSavingsInterestAsync();

            if (error != null)
            {
                return BadRequest(new { message = error });
            }

            return Ok(new { message = "Tính lãi thành công cho các sổ tiết kiệm." });
        }


        [HttpGet("View-Saving")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _staffService.GetAllSavingsAsync();
            return Ok(list);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetAllHistories()
        {
            var result = await _staffService.GetFormattedHistoriesAsync(); // trả về List<TransactionHistoryDTO>
            return Ok(result);
        }

        [HttpGet("matured-savings")]
        public async Task<IActionResult> GetMaturedSavings()
        {
            var result = await _staffService.GetMaturedSavingsWithWithdrawnAsync();
            return Ok(result);
        }

    }
}
