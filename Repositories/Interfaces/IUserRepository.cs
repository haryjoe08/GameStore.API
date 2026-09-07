using GameStoreApi.Models;

namespace GameStoreApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<bool> ExistsAsync(string username);
    Task UpdateAsync(User user);
}