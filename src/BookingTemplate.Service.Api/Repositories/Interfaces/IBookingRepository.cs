using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<List<TimeSlot>> GetAvailable();
    Task Book(TimeSlot timeSlot, int userId);
    Task<TimeSlot?> GetTimeSlot(int id);
    Task<List<TimeSlot>> GetBookingsByUserId(int userId);
    Task CancelBooking(TimeSlot timeSlot);
}
