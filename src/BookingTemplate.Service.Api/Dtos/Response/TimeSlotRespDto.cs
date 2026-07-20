namespace BookingTemplate.Service.Api.Dtos.Response;

public class TimeSlotRespDto
{
    public int Id { get; set; }
    public required DateTime StartTimeUtc { get; set; }
    public required DateTime EndTimeUtc { get; set; }
    public bool IsBooked { get; set; }
    public string? Notes { get; set; }
    public int? BookedByUserId { get; set; }
    public string? BookedByUserEmail { get; set; }
}