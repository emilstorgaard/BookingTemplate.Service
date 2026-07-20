using BookingTemplate.Service.Api.Dtos.Response;

namespace BookingTemplate.Service.Api.Services.Interfaces;

public interface IBookingService
{
    Task<List<TimeSlotRespDto>> GetAvailable();
    Task Book(int timeSlotId, int userId);
    Task<List<TimeSlotRespDto>> GetBookinsByUserId(int userId);
    Task CancelBooking(int id, int userId);
}
