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
                _logger.LogWarning($"[Login Failed] Email: {loginRequest.Email}");
                return Unauthorized(new { message = "Email hoặc mật khẩu không chính xác." });
            }

            var token = _jwtHelper.GenerateJwtToken(user);
            _jwtHelper.SetJwtCookie(Response, token);

            _logger.LogInformation($"[Login Success] User: {user.Email}");
            return Ok(new { message = "Đăng nhập thành công!" });
        }

        /// <summary> API Lấy thông tin người dùng từ JWT </summary>
        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = _jwtHelper.ValidateJwtFromCookie(Request);
            if (userId == null)
            {
                _logger.LogWarning("🚨 Token không hợp lệ hoặc không tìm thấy.");
                return Unauthorized(new { message = "Chưa đăng nhập hoặc Token không hợp lệ." });
            }

            var user = await _userRepository.GetUserById(userId.Value);
            if (user == null)
            {
                _logger.LogWarning($"🚨 Không tìm thấy User với ID: {userId}");
                return NotFound(new { message = "User không tồn tại." });
            }

            _logger.LogInformation($"✅ Lấy thông tin User: {user.Email}");
            return Ok(user);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1), // 🔥 Hết hạn ngay lập tức
                Path = "/", // 🔥 Phải trùng với Path khi tạo cookie
                Domain = "xekhach.click", // 🔥 Phải trùng với domain khi tạo cookie
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            });

            _logger.LogInformation("✅ Đã đăng xuất thành công.");
            return Ok(new { message = "Đăng xuất thành công." });
        }



        /// <summary>
        /// Tạo người dùng mới với mật khẩu được mã hóa
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mã hóa mật khẩu trước khi lưu
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            await _userRepository.AddUser(user);
            return CreatedAtAction(nameof(GetUserProfile), new { id = user.Id }, user);
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (user.Id == 0)
                user.Id = id;
            else if (id != user.Id)
                return BadRequest(new { message = "ID in URL does not match ID in body." });

            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null)
                return NotFound(new { message = "User not found." });

            // Nếu mật khẩu mới được cung cấp, mã hóa nó
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }
            else
            {
                user.PasswordHash = existingUser.PasswordHash; // Giữ nguyên mật khẩu cũ
            }

            await _userRepository.UpdateUser(user);
            return NoContent();
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
