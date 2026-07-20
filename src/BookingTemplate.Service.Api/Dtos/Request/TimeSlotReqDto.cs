namespace BookingTemplate.Service.Api.Dtos.Request;

public class TimeSlotReqDto
{
    public required DateTime StartTimeUtc { get; set; }
    public required DateTime EndTimeUtc { get; set; }
    public string? Notes { get; set; }
}

public class BulkTimeSlotReqDto
{
    public required DateTime FromUtc { get; set; }
    public required DateTime ToUtc { get; set; }
    public required int SlotLengthMinutes { get; set; }
}