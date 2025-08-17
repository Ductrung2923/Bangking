using Banking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace Banking.API.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileAdminController : ControllerBase
    {

       private readonly AdminServices _adminService;

        public ProfileAdminController(AdminServices adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = "1")] 
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminProfile()
        {
            // Lấy userId từ claim (thường là "sub" nếu bạn gán trong JWT)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)
                            ?? User.FindFirst(JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                return Unauthorized(new { Message = "Token không hợp lệ hoặc thiếu thông tin." });

            var userId = int.Parse(userIdClaim.Value);

            var adminInfo = await _adminService.GetAdminInfo(userId);
            if (adminInfo == null) return NotFound(new { Message = "Không tìm thấy thông tin Admin." });

            return Ok(adminInfo);
        }
    }
}
