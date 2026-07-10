using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookingTemplate.Service.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenRespDto>> Login([FromForm] LoginReqDto loginReqDto)
    {
        var result = await _authService.Login(loginReqDto.Email, loginReqDto.Password);
        return result;
    }
}

