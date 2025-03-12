using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

public class JwtHelper
{
    private readonly IConfiguration _config;
    private readonly ILogger<JwtHelper> _logger;

    public JwtHelper(IConfiguration config, ILogger<JwtHelper> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <summary> Tạo JWT Token </summary>
    public string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary> Lưu JWT vào HttpOnly Cookie </summary>
    public void SetJwtCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // 🔒 Chặn truy cập từ JavaScript
            Secure = true,  // ✅ Bắt buộc `true` khi chạy HTTPS
            SameSite = SameSiteMode.None, // ✅ Bắt buộc dùng `None` khi `Secure=true`
            Expires = DateTime.UtcNow.AddHours(1),
            Path = "/", // 🔥 Đảm bảo cookie áp dụng cho toàn bộ trang web
            Domain = "xekhach.click" // 🔥 Cập nhật domain theo trang web của bạn
        };

       if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        cookieOptions.Domain = null; // ✅ Để trình duyệt tự quyết định domain
    }

    response.Cookies.Append("jwt", token, cookieOptions);
    }


    /// <summary> Xác thực JWT từ Cookie </summary>
    public int? ValidateJwtFromCookie(HttpRequest request)
    {
        if (!request.Cookies.TryGetValue("jwt", out var token))
        {
            _logger.LogWarning("🚨 Không tìm thấy JWT trong Cookie!");
            return null;
        }

        try
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var userIdClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"🚨 Lỗi xác thực Token: {ex.Message}");
            return null;
        }
    }
}
