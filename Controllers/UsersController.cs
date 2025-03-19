using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using LuanAnTotNghiep_TuanVu_TuBac.Models.Enums;
using LuanAnTotNghiep_TuanVu_TuBac.Services.Interfaces;
using LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs;

namespace LuanAnTotNghiep_TuanVu_TuBac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;
        private readonly JwtHelper _jwtHelper;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _jwtHelper = jwtHelper;
        }

        /// <summary> API Đăng nhập, lưu JWT vào HttpOnly Cookie </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Models.DTOs.LoginRequest loginRequest)
        {
            var user = await _userRepository.GetUserByEmail(loginRequest.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác." });
            }

            var token = _jwtHelper.GenerateJwtToken(user);
            _jwtHelper.SetJwtCookie(Response, token);

            return Ok(new { message = "Đăng nhập thành công!" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Models.DTOs.RegisterRequest registerRequest)
        {
            // Kiểm tra xem email đã tồn tại chưa
            var existingUser = await _userRepository.GetUserByEmail(registerRequest.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Email này đã được sử dụng." });
            }

            // Kiểm tra số điện thoại có bị trùng không
            var existingPhoneNumber = await _userRepository.GetUserByPhoneNumber(registerRequest.PhoneNumber);
            if (existingPhoneNumber != null)
            {
                return BadRequest(new { message = "Số điện thoại này đã được sử dụng." });
            }

            // Mã hóa mật khẩu
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            // Tạo user mới
            var newUser = new User
            {
                Email = registerRequest.Email,
                FullName = registerRequest.FullName,
                PasswordHash = passwordHash,
                Gender = registerRequest.Gender,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = registerRequest.Address,
                Role = (byte)UserRole.User, // Sử dụng Enum để tránh lỗi kiểu dữ liệu
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TokenVersion = 1
            };

            await _userRepository.AddUser(newUser);

            return Ok(new { message = "Đăng ký thành công! Vui lòng đăng nhập." });
        }

        /// <summary> API Lấy thông tin người dùng từ JWT </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = await _jwtHelper.ValidateJwtFromCookie(Request, _userRepository);
            if (userId == null)
            {
                return Unauthorized(new { message = "Chưa đăng nhập hoặc Token không hợp lệ." });
            }

            var user = await _userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User không tồn tại." });
            }

            // Chuyển đổi sang DTO
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                BirthDate = user.BirthDate?.ToString("yyyy-MM-dd"),
                Gender = user.Gender,
                Address = user.Address
            };

            return Ok(userResponse);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserRequest request)
        {
            var userId = await _jwtHelper.ValidateJwtFromCookie(Request, _userRepository);
            if (userId == null)
            {
                return Unauthorized(new { message = "Chưa đăng nhập hoặc Token không hợp lệ." });
            }

            var user = await _userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User không tồn tại." });
            }

            // Cập nhật các trường cho phép sửa đổi
            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;
            user.BirthDate = request.BirthDate;
            user.Gender = request.Gender;
            user.Address = request.Address;
            user.UpdatedAt = DateTime.UtcNow;

            // Gọi repository để cập nhật
            var result = await _userRepository.UpdateUser(user);
            if (!result)
            {
                return StatusCode(500, new { message = "Lỗi khi cập nhật thông tin." });
            }

            return Ok(new { message = "Cập nhật thông tin thành công." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Cookies.TryGetValue("jwt", out var token))
            {
                return Unauthorized(new { message = "Bạn chưa đăng nhập!" });
            }

            var userId = await _jwtHelper.ValidateJwtFromCookie(Request, _userRepository);
            if (userId == null)
            {
                return Unauthorized(new { message = "Token không hợp lệ!" });
            }

            // ✅ Tăng TokenVersion để vô hiệu hóa tất cả token cũ
            var user = await _userRepository.GetUserById(userId.Value);
            if (user != null)
            {
                user.TokenVersion++;
                await _userRepository.UpdateUserAsync(user);
            }

            // ✅ Xóa cookie JWT
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/",
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Đăng xuất thành công!" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs.ForgotPasswordRequest request,
            [FromServices] IEmailService emailService)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email không tồn tại trong hệ thống." });
            }

            // ✅ Tạo mã OTP 6 số
            var otp = new Random().Next(100000, 999999).ToString();
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5); // Hết hạn sau 5 phút
            await _userRepository.UpdateUserAsync(user);

            // ✅ Gửi email OTP
            var emailMessage = $"<h3>OTP của bạn là: <b>{otp}</b></h3><p>OTP có hiệu lực trong 5 phút.</p>";
            await emailService.SendEmailAsync(user.Email, "Xác nhận OTP - Quên Mật Khẩu", emailMessage);

            return Ok(new { message = "Đã gửi mã OTP đến email của bạn." });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || user.OtpCode != request.OtpCode || user.OtpExpiry < DateTime.UtcNow)
            {
                return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn." });
            }

            return Ok(new { message = "Xác thực OTP thành công. Vui lòng đặt lại mật khẩu mới." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] Models.DTOs.ResetPasswordRequest request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);
            if (user == null || user.OtpExpiry < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Yêu cầu đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.OtpCode = null;
            user.OtpExpiry = null;
            await _userRepository.UpdateUserAsync(user);

            return Ok(new { message = "Mật khẩu đã được cập nhật thành công!" });
        }

       

        /// <summary>
        /// Xóa người dùng theo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null)
                return NotFound(new { message = "User not found." });

            await _userRepository.DeleteUser(id);
            return NoContent();
        }
    }
}
