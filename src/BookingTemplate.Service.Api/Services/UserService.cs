using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Exceptions;
using BookingTemplate.Service.Api.Helpers;
using BookingTemplate.Service.Api.Mappers;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using BookingTemplate.Service.Api.Services.Interfaces;

namespace BookingTemplate.Service.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserRespDto>> GetAll()
    {
        var users = await _userRepository.GetAllUsers();
        if (!users.Any()) throw new NotFoundException("No users found.");

        var userDtos = users.Select(UserMapper.MapToDto).ToList();
        return userDtos;
    }

    public async Task<UserRespDto> GetUser(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) throw new NotFoundException("User not found.");

        var userDto = UserMapper.MapToDto(user);
        return userDto;
    }

    public async Task AddUser(UserReqDto userReqDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(userReqDto.Email);
        if (existingUser != null) throw new ConflictException("User with this email already exists.");

        var passwordHash = PasswordHelper.HashPassword(userReqDto.Password);

        var defaultRole = new Role { Id = 2, Name = "User" };

        var user = new User
        {
            Email = userReqDto.Email,
            PasswordHash = passwordHash,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
            Roles = new List<Role> { defaultRole }
        };

        await _userRepository.AddUser(user);
    }

    public async Task Delete(int userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user == null) throw new NotFoundException("User not found.");

        await _userRepository.Delete(user);
    }
}

