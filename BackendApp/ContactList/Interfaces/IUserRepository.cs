using ContactList.Entities;

namespace ContactList.Interfaces;

public interface IUserRepository
{
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<User> InsertUserAsync(User user);
    public Task SaveUserChangesAsync();
}