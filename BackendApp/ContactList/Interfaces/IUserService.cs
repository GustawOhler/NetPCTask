using ContactList.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactList.Interfaces;

public interface IUserService
{
    Task<IdentityResult> Register(string username, string email, string password);
    Task<User?> Login(string username, string password);
}