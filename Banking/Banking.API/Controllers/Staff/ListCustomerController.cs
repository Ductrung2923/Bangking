using Banking.Application.DTOs;
using Banking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListCustomerController : ControllerBase
    {
        private readonly StaffService _staffService;

        public ListCustomerController(StaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerDTO>>> GetAllCustomers()
        {
            var customers = await _staffService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid customer data.");


            var result = await _staffService.AddCustomerAsync(dto);

            if (result)
                return Ok(new { message = "Thêm khách hàng thành công và đã gửi email." });
            else
                return StatusCode(500, "Thêm khách hàng thất bại.");
        }

    }
}
