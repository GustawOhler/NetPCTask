using ContactList.Entities;
using ContactList.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactList.Repositories;

public class UserRepository : IUserRepository
{
    private DbContext _context;

    public UserRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }

    public async Task<User> InsertUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task SaveUserChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}