using BookingTemplate.Service.Api.Data;
using BookingTemplate.Service.Api.Entities;
using BookingTemplate.Service.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingTemplate.Service.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _dbContext.Users
            .Include(u => u.Roles)
            .ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _dbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddUser(User user)
    {
        foreach (var role in user.Roles)
        {
            _dbContext.Entry(role).State = EntityState.Unchanged;
        }

        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Delete(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
}
