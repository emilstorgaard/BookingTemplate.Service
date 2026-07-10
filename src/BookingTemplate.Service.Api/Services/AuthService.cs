using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Exceptions;
using BookingTemplate.Service.Api.Helpers;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using BookingTemplate.Service.Api.Services.Interfaces;

namespace BookingTemplate.Service.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _tokenService;

    public AuthService(IUserRepository userRepository, IJwtTokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenRespDto> Login(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash)) throw new UnauthorizedException("Invalid email or password.");

        var token = _tokenService.GenerateToken(user);

        var tokenRespDto = new TokenRespDto
        {
            Token = token
        };

        return tokenRespDto;
    }
}