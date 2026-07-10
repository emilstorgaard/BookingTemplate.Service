namespace BookingTemplate.Service.Api.Entities;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
    public List<Role> Roles { get; set; } = new();
}