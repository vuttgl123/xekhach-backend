using LuanAnTotNghiep_TuanVu_TuBac.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces;
using LuanAnTotNghiep_TuanVu_TuBac.Repositories.Implementations;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<JwtHelper>();

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Logging.AddConsole();

builder.Services.AddAuthorization();

// 🌐 Cấu hình CORS cho Frontend (React)
var allowedOrigins = new string[]
{
    "https://xekhach.click",
    "https://www.xekhach.click",
    "http://localhost:5173"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins(allowedOrigins)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});

var app = builder.Build();

// ✅ Bật Swagger trên cả môi trường Production
app.UseCors("AllowFrontend");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // 🔥 Thêm Authentication vào Middleware
app.UseAuthorization();

// ✅ Lắng nghe cổng từ biến môi trường (Render cấp cổng)
var port = Environment.GetEnvironmentVariable("PORT") ?? "5120";
app.Urls.Add($"http://*:{port}");

// ✅ Route mặc định kiểm tra API có chạy không
app.MapGet("/", () => "🚀 API is running on Render!");

app.MapControllers();

app.Run();
