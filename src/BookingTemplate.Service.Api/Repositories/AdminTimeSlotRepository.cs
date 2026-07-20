using BookingTemplate.Service.Api.Data;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingTemplate.Service.Api.Repositories;

public class AdminTimeSlotRepository : IAdminTimeSlotRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AdminTimeSlotRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddTimeSlot(TimeSlot timeSlot)
    {
        _dbContext.TimeSlots.Add(timeSlot);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddBulk(List<TimeSlot> timeSlots)
    {
        _dbContext.TimeSlots.AddRange(timeSlots);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<TimeSlot>> GetAll()
    {
        return await _dbContext.TimeSlots
            .Include(s => s.BookedByUser)
            .OrderBy(s => s.StartTimeUtc)
            .ToListAsync();
    }

    public async Task<TimeSlot?> GetTimeSlot(int id)
    {
        return await _dbContext.TimeSlots.FindAsync(id);
    }

    public async Task Delete(TimeSlot timeSlot)
    {
        _dbContext.TimeSlots.Remove(timeSlot);
        await _dbContext.SaveChangesAsync();
    }
}
