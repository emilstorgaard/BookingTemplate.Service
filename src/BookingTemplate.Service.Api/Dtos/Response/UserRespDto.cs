using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Dtos.Response;

public class UserRespDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
