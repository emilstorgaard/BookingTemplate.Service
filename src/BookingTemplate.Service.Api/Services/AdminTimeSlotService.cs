using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Exceptions;
using BookingTemplate.Service.Api.Mappers;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using BookingTemplate.Service.Api.Services.Interfaces;

namespace BookingTemplate.Service.Api.Services;

public class AdminTimeSlotService : IAdminTimeSlotService
{
    private readonly IAdminTimeSlotRepository _adminTimeSlotRepository;

    public AdminTimeSlotService(IAdminTimeSlotRepository adminTimeSlotRepository)
    {
        _adminTimeSlotRepository = adminTimeSlotRepository;
    }

    public async Task AddTimeSlot(TimeSlotReqDto timeSlotReqDto, int currentUserId)
    {
        if (timeSlotReqDto.EndTimeUtc <= timeSlotReqDto.StartTimeUtc)
            throw new BadRequestException("EndTimeUtc skal ligge efter StartTimeUtc.");

        var timeSlot = new TimeSlot
        {
            StartTimeUtc = timeSlotReqDto.StartTimeUtc,
            EndTimeUtc = timeSlotReqDto.EndTimeUtc,
            Notes = timeSlotReqDto.Notes,
            CreatedByAdminId = currentUserId,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _adminTimeSlotRepository.AddTimeSlot(timeSlot);
    }

    public async Task AddBulk(BulkTimeSlotReqDto bulkTimeSlotReqDto, int currentUserId)
    {
        if (bulkTimeSlotReqDto.ToUtc <= bulkTimeSlotReqDto.FromUtc || bulkTimeSlotReqDto.SlotLengthMinutes <= 0)
            throw new BadRequestException("Ugyldigt interval eller varighed.");

        var now = DateTime.UtcNow;
        var timeSlots = new List<TimeSlot>();

        for (var start = bulkTimeSlotReqDto.FromUtc;
             start.AddMinutes(bulkTimeSlotReqDto.SlotLengthMinutes) <= bulkTimeSlotReqDto.ToUtc;
             start = start.AddMinutes(bulkTimeSlotReqDto.SlotLengthMinutes))
        {
            timeSlots.Add(new TimeSlot
            {
                StartTimeUtc = start,
                EndTimeUtc = start.AddMinutes(bulkTimeSlotReqDto.SlotLengthMinutes),
                CreatedByAdminId = currentUserId,
                CreatedAtUtc = now
            });
        }

        await _adminTimeSlotRepository.AddBulk(timeSlots);
    }

    public async Task<List<TimeSlotRespDto>> GetAll()
    {
        var timeSlots = await _adminTimeSlotRepository.GetAll();

        var timeSlotsDto = timeSlots.Select(TimeSlotMapper.MapToDto).ToList();
        return timeSlotsDto;
    }

    public async Task<TimeSlotRespDto> GetTimeSlot(int id)
    {
        var timeSlot = await _adminTimeSlotRepository.GetTimeSlot(id);

        if (timeSlot is null)
            throw new NotFoundException($"TimeSlot med id: {id} blev ikke fundet");

        var timeSlotDto = TimeSlotMapper.MapToDto(timeSlot);
        return timeSlotDto;
    }

    public async Task Delete(int id)
    {
        var timeSlot = await _adminTimeSlotRepository.GetTimeSlot(id);
        if (timeSlot == null) throw new NotFoundException("TimeSlot not found.");
        if (timeSlot.IsBooked) throw new ConflictException("Tiden er allerede booket og kan ikke slettes. Aflys booking'en først.");

        await _adminTimeSlotRepository.Delete(timeSlot);
    }
}
