using Banking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Banking.Application.DTOs;

namespace Banking.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateProfileAdminController : ControllerBase
    {
        private readonly AdminServices _adminService;

        public UpdateProfileAdminController(AdminServices adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "1")] // Chỉ Admin mới được cập nhật
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                             ?? User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
                return Unauthorized(new { Message = "Token không hợp lệ." });

            int userId = int.Parse(userIdClaim.Value);

            var success = await _adminService.UpdateAdminProfile(userId, dto);
            if (!success)
                return NotFound(new { Message = "Không tìm thấy admin để cập nhật." });

            return Ok(new { Message = "Cập nhật thành công!" });
        }

    }
}
