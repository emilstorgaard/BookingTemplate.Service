using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
