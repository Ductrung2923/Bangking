using Banking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeaddController : ControllerBase
    {
        private readonly AdminServices _adminServices;

        public HomeaddController(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("newcustomer")]
        public async Task<IActionResult> GetRecentCustomers()
        {
            var result = await _adminServices.GetRecentCustomersAsync();
            return Ok(result);
        }

        [HttpGet("total-staff")]
        public async Task<IActionResult> GetTotalStaff()
        {
            int count = await _adminServices.GetTotalStaffAsync();
            return Ok(count);
        }

        [HttpGet("total-customers")]
        public async Task<IActionResult> GetTotalCustomers()
        {
            int count = await _adminServices.GetTotalCustomersAsync();
            return Ok(count);
        }

        [HttpGet("total-accounts")]
        public async Task<IActionResult> GetTotalAccounts()
        {
            int count = await _adminServices.GetTotalAccountsAsync();
            return Ok(count);
        }

        [HttpGet("total-externalbanks")]
        public async Task<IActionResult> GetTotalExternalBanks()
        {
            int count = await _adminServices.GetTotalExternalBanksAsync();
            return Ok(count);
        }

        [HttpGet("weekly-transactions")]
        public async Task<IActionResult> GetWeeklyTransactionStats()
        {
            var data = await _adminServices.GetWeeklyTransactionStatsAsync();
            return Ok(data);
        }

        [HttpGet("total-customer-balance")]
        public async Task<IActionResult> GetTotalCustomerBalance()
        {
            var total = await _adminServices.GetTotalCustomerBalanceAsync();
            return Ok(new { totalBalance = total });
        }


    }
}
