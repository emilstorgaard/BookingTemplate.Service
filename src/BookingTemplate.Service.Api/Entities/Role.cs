namespace BookingTemplate.Service.Api.Entities;

public class Role
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<User> Users { get; set; } = new();
}
