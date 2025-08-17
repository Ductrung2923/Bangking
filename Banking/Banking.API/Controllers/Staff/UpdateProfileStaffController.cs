using Banking.Application.DTOs;
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
    public class UpdateProfileStaffController : ControllerBase
    {
        private readonly StaffService _staffService;

        public UpdateProfileStaffController(StaffService staffService)
        {
            _staffService = staffService;
        }

        [Authorize(Roles = "0")] 
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                             ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized(new { Message = "Token không hợp lệ." });

            int userId = int.Parse(userIdClaim.Value);

            var success = await _staffService.UpdateStaffProfile(userId,dto);
            if (!success)
                return NotFound(new { Message = "Không tìm thấy admin để cập nhật." });

            return Ok(new { Message = "Cập nhật thành công!" });
        }
    }
}



