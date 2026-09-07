using GameStoreApi.Data;
using GameStoreApi.Models;
using GameStoreApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GameStoreDbContext _dbContext;

    public UserRepository(GameStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsAsync(string username)
    {
        return await _dbContext.Users
            .AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }
    
    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
}