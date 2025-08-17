using Microsoft.EntityFrameworkCore;
using Banking.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Banking.Infrastructure.Repositories;
using Banking.Application.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Banking.Infrastructure.Models;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Load cấu hình SMTP
var smtpSettings = config.GetSection("SmtpSettings").Get<SmtpSettings>();
services.AddSingleton(smtpSettings);

// Đăng ký các dịch vụ
services.AddTransient<EmailService>();
services.AddMemoryCache();
services.AddScoped<OTPService>();
services.AddScoped<UserRepository>();
services.AddSingleton<JwtService>();
services.AddScoped<UserService>();
services.AddScoped<AdminServices>();
services.AddScoped<AdminRepository>();
services.AddScoped<StaffService>();
services.AddScoped<StaffRepository>();
services.AddScoped<CustomerRepository>();
services.AddScoped<AccountRepository>();
services.AddScoped<TransactionRepository>();
services.AddScoped<TransactionHistoryRepository>();
services.AddScoped<ExtraAccountRepository>();
services.AddScoped<SavingRepository>();





// Cấu hình DbContext
services.AddDbContext<Banking.Infrastructure.Data.BankingApiContext>(options =>
    options.UseSqlServer(config.GetConnectionString("MyCnn")));

// ✅ Cấu hình Authentication (JWT + Cookie + Google)
services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // JWT là mặc định
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]))
    };
})
.AddCookie() // Cookie hỗ trợ đăng nhập Google
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    options.ClientId = config["GoogleKey:ClientId"];
    options.ClientSecret = config["GoogleKey:ClientSecret"];
    options.CallbackPath = "/signin-google";
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200") // 👈 Angular chạy ở đây
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Banking API",
        Version = "v1"
    });

    // ✅ Cấu hình để hiển thị nút Authorize với JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập chuỗi token dạng: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
