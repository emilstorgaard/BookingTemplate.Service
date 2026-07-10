using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Mappers;

public static class UserMapper
{
    public static UserRespDto MapToDto(User user)
    {
        return new UserRespDto
        {
            Id = user.Id,
            Email = user.Email,
            Roles = user.Roles.Select(r => r.Name).ToList(),
            CreatedAtUtc = user.CreatedAtUtc,
            UpdatedAtUtc = user.UpdatedAtUtc
        };
    }
}