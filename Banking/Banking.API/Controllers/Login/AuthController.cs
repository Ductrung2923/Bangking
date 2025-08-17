using Banking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Banking.Application.DTOs;
using Banking.Infrastructure.Models;

namespace Banking.API.Controllers.Login
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Banking.Application.DTOs.LoginRequest request)
        {
            var token = _userService.Authenticate(request.Username, request.Password);
            

            if (token == "INACTIVE")
            {
                return Unauthorized(new { Message = "Tài khoản của bạn chưa được kích hoạt." });
            }

            if (token == null) return Unauthorized(new { Message = "Thông tin đăng nhập không đúng." });
            return Ok(new { Token = token});
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Banking.Application.DTOs.ResetPasswordDTO re)
        {
            if (string.IsNullOrWhiteSpace(re.Username))
            {
                return BadRequest(new { Message = "Username không được để trống." });
            }

            var result = await _userService.ResetPasswordAsync(re.Username);

            if (result == "Tài khoản không tồn tại.")
            {
                return BadRequest(new { Message = result });
            }

            if (result == "Không thể cập nhật mật khẩu.")
            {
                return StatusCode(500, new { Message = result });
            }

            return Ok(new { Message = result });
        }





    }
}
