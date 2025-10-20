using ContactList.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactList.Interfaces;

interface IUserService
{
    Task<IdentityResult> Register(string username, string email, string password);
    Task<string?> Login(string username, string password);
}