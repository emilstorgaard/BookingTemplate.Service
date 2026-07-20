
using BookingTemplate.Service.Api.Configurations;
using BookingTemplate.Service.Api.Data;
using BookingTemplate.Service.Api.Extensions;
using BookingTemplate.Service.Api.Middleware;
using BookingTemplate.Service.Api.Repositories;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using BookingTemplate.Service.Api.Services;
using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace BookingTemplate.Service.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration
                .AddJsonFile(
                    "local.settings.json",
                    optional: true,
                    reloadOnChange: true
                );
        }

        ConfigureServices(builder);
        ConfigureAuthentication(builder);
        ConfigureCors(builder);

        var app = builder.Build();
        ConfigurePipeline(app);

        app.Run();
    }

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException("Database connection string is not configured.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddSingleton(sp => {
            var configuration = sp.GetRequiredService<IConfiguration>();

            var settings = new Settings
            {
                JwtSecret = configuration["JwtSettings:Secret"] ?? throw new NullReferenceException($"No {nameof(configuration)} found in configuration!"),
                JwtExpiryHours = int.TryParse(configuration["JwtSettings:ExpiryHours"], out var expiry) ? expiry : throw new NullReferenceException($"No {nameof(configuration)} found in configuration!")
            };

            return settings;
        });


        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAdminTimeSlotRepository, AdminTimeSlotRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAdminTimeSlotService, AdminTimeSlotService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
    }

    public static void ConfigureAuthentication(WebApplicationBuilder builder)
    {
        var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? throw new NullReferenceException("JWT Secret is not configured.");

        var key = Encoding.ASCII.GetBytes(jwtSecret);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }

    public static void ConfigureCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
    }

    public static void ConfigurePipeline(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowAllOrigins");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.MapControllers();
    }
}
