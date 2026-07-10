using BookingTemplate.Service.Api.Configurations;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingTemplate.Service.Api.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly Settings _settings;

    public JwtTokenService(Settings settings)
    {
        _settings = settings;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_settings.JwtSecret);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        if (user.Roles != null)
        {
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(_settings.JwtExpiryHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}