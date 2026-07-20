using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Mappers;

public class TimeSlotMapper
{
    public static TimeSlotRespDto MapToDto(TimeSlot timeSlot)
    {
        return new TimeSlotRespDto
        {
            Id = timeSlot.Id,
            StartTimeUtc = timeSlot.StartTimeUtc,
            EndTimeUtc = timeSlot.EndTimeUtc,
            IsBooked = timeSlot.IsBooked,
            Notes = timeSlot.Notes,
            BookedByUserId = timeSlot.BookedByUserId,
            BookedByUserEmail = timeSlot.BookedByUser?.Email
        };
    }
}
