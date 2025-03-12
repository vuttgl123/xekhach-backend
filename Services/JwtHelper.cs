using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
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

    public JwtHelper(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("TokenVersion", user.TokenVersion.ToString()) // ✅ Thêm TokenVersion
        };

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1), // 🔥 Token hết hạn sau 1 giờ
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public void SetJwtCookie(HttpResponse response, string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        response.Cookies.Append("jwt", token, cookieOptions);
    }

    public async Task<int?> ValidateJwtFromCookie(HttpRequest request, IUserRepository userRepository)
    {
        if (!request.Cookies.TryGetValue("jwt", out var token))
        {
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
            var tokenVersionClaim = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "TokenVersion");

            if (userIdClaim == null || tokenVersionClaim == null)
            {
                return null;
            }

            int userId = Convert.ToInt32(userIdClaim.Value);
            int tokenVersion = Convert.ToInt32(tokenVersionClaim.Value);

            // ✅ Kiểm tra nếu TokenVersion trong database khớp với token
            var user = await userRepository.GetUserById(userId);
            if (user == null || user.TokenVersion != tokenVersion)
            {
                return null;
            }

            return userId;
        }
        catch (Exception)
        {
            return null;
        }
    }
}

