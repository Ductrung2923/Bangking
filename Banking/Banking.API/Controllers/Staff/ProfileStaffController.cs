using Banking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Banking.API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileStaffController : ControllerBase
    {
        private readonly StaffService _staffService;

        public ProfileStaffController(StaffService staffService)
        {
            _staffService = staffService;
        }

        [Authorize(Roles = "0")]
        [HttpGet("staff")]
        public async Task<IActionResult> GetAdminProfile()
        {
           
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                            ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                return Unauthorized(new { Message = "Token không hợp lệ hoặc thiếu thông tin." });

            var userId = int.Parse(userIdClaim.Value);

            var staffInfo = await _staffService.GetStaffInfo(userId);
            if (staffInfo == null) return NotFound(new { Message = "Không tìm thấy thông tin Admin." });

            return Ok(staffInfo);
        }
    }
}
