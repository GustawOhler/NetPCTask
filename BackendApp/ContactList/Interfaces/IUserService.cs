using ContactList.DTOs;
using ContactList.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactList.Interfaces;

public interface IUserService
{
    Task<OperationResult<IdentityResult>> Register(RegisterRequest registerRequest);
    Task<OperationResult<User>> Login(string username, string password);
}