using BookingTemplate.Service.Api.Data;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingTemplate.Service.Api.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _dbContext;

    public BookingRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TimeSlot>> GetAvailable()
    {
        return await _dbContext.TimeSlots
            .Where(s => !s.IsBooked && s.StartTimeUtc > DateTime.UtcNow)
            .OrderBy(s => s.StartTimeUtc)
            .ToListAsync();
    }

    public async Task Book(TimeSlot timeSlot, int userId)
    {
        timeSlot.IsBooked = true;
        timeSlot.BookedByUserId = userId;
        timeSlot.BookedAtUtc = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<TimeSlot?> GetTimeSlot(int id)
    {
        return await _dbContext.TimeSlots.FindAsync(id);
    }

    public async Task<List<TimeSlot>> GetBookingsByUserId(int userId)
    {
        return await _dbContext.TimeSlots
            .Where(s => s.BookedByUserId == userId)
            .OrderBy(s => s.StartTimeUtc)
            .ToListAsync();
    }

    public async Task CancelBooking(TimeSlot timeSlot)
    {
        timeSlot.IsBooked = false;
        timeSlot.BookedByUserId = null;
        timeSlot.BookedAtUtc = null;

        await _dbContext.SaveChangesAsync();
    }
}
