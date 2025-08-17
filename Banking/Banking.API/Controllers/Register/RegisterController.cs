using Banking.Application.DTOs;
using Banking.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Banking.API.Controllers.Register
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private UserService _userService;

        public RegisterController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("step1")]
        public async Task<IActionResult> RegisterStep1([FromBody] RegisterDTO request)
        {
            var result = await _userService.RegisterStep1Async(request);

            return Ok(new { message = result });
        }


        [HttpPost("step2")]
        public async Task<IActionResult> RegisterStep2([FromBody] ConfirmOtpDTO request)
        {
            var token = await _userService.RegisterStep2Async(request.Otp);
            if (token == null)
                return BadRequest(new { Message = "OTP không hợp lệ hoặc đã hết hạn" });

            return Ok(new { Message = "Tạo tài khoản thành công", Token = token });
        }





    }
}
