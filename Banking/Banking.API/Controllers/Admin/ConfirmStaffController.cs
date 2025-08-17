using Banking.Application.DTOs;
using Banking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmStaffController : ControllerBase
    {
        private readonly AdminServices _adminServices;

        public ConfirmStaffController(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<StaffDTO>>> GetAllStaffs()
        {
            var staffList = await _adminServices.GetAllStaffAccounts();
            return Ok(staffList);
        }

      
        [HttpPut("{userId}")]
        public async Task<IActionResult> ConfirmStaff(int userId, [FromBody] bool isActive)
        {
            var success = await _adminServices.ApproveStaffAccount(userId, isActive);
            if (!success)
                return NotFound(new { message = "User not found." });

            return Ok(new { message = "Staff account updated successfully." });
        }
    }
}
