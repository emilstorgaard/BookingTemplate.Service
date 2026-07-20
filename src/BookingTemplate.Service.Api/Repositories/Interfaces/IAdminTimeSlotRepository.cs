using BookingTemplate.Service.Api.Entities;

namespace BookingTemplate.Service.Api.Repositories.Interfaces;

public interface IAdminTimeSlotRepository
{
    Task AddTimeSlot(TimeSlot timeSlot);
    Task AddBulk(List<TimeSlot> timeSlots);
    Task<List<TimeSlot>> GetAll();
    Task<TimeSlot?> GetTimeSlot(int id);
    Task Delete(TimeSlot timeSlot);
}
