using BookingTemplate.Service.Api.Dtos.Response;

namespace BookingTemplate.Service.Api.Services.Interfaces;

public interface IAuthService
{
    Task<TokenRespDto> Login(string email, string password);
}