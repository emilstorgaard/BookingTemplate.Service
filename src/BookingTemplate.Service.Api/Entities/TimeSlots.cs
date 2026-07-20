using System.ComponentModel.DataAnnotations;

namespace BookingTemplate.Service.Api.Entities;

public class TimeSlot
{
    public int Id { get; set; }
    public DateTime StartTimeUtc { get; set; }
    public DateTime EndTimeUtc { get; set; }
    public bool IsBooked { get; set; }

    public int? BookedByUserId { get; set; }
    public User? BookedByUser { get; set; }
    public DateTime? BookedAtUtc { get; set; }

    public int CreatedByAdminId { get; set; }
    public User? CreatedByAdmin { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}
