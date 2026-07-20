using BookingTemplate.Service.Api.Dtos.Response;
using BookingTemplate.Service.Api.Exceptions;
using BookingTemplate.Service.Api.Mappers;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingTemplate.Service.Api.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<List<TimeSlotRespDto>> GetAvailable()
    {
        var availableTimeSlots = await _bookingRepository.GetAvailable();

        var availableTimeSlotsDto = availableTimeSlots.Select(TimeSlotMapper.MapToDto).ToList();
        return availableTimeSlotsDto;
    }

    public async Task Book(int timeSlotId, int userId)
    {
        var timeSlot = await _bookingRepository.GetTimeSlot(timeSlotId);
        if (timeSlot is null)
            throw new NotFoundException($"TimeSlot med id: {timeSlotId} blev ikke fundet");

        if (timeSlot.IsBooked)
            throw new ConflictException("Tiden er desværre allerede booket.");

        if (timeSlot.StartTimeUtc <= DateTime.UtcNow)
            throw new BadRequestException("Tiden er overstået.");

        try
        {
            await _bookingRepository.Book(timeSlot, userId);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Tiden er desværre lige blevet booket af en anden.");
        }
    }

    public async Task<List<TimeSlotRespDto>> GetBookinsByUserId(int userId)
    {
        var bookings = await _bookingRepository.GetBookingsByUserId(userId);

        var bookingsTimeSlotsDto = bookings.Select(TimeSlotMapper.MapToDto).ToList();
        return bookingsTimeSlotsDto;
    }

    public async Task CancelBooking(int id, int userId)
    {
        var timeSlot = await _bookingRepository.GetTimeSlot(id);
        if (timeSlot is null || timeSlot.BookedByUserId != userId) throw new NotFoundException("Booking blev ikke fundet for brugeren");

        await _bookingRepository.CancelBooking(timeSlot);
    }
}
