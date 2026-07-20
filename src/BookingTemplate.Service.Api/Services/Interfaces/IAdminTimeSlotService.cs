using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Dtos.Response;

namespace BookingTemplate.Service.Api.Services.Interfaces;

public interface IAdminTimeSlotService
{
    Task AddTimeSlot(TimeSlotReqDto timeSlotReqDto, int currentUserId);
    Task AddBulk(BulkTimeSlotReqDto bulkTimeSlotReqDto, int currentUserId);
    Task<List<TimeSlotRespDto>> GetAll();
    Task<TimeSlotRespDto> GetTimeSlot(int id);
    Task Delete(int id);
}
