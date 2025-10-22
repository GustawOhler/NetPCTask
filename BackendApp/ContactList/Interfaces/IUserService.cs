using ContactList.DTOs;
using ContactList.Entities;
using Microsoft.AspNetCore.Identity;

namespace ContactList.Interfaces;

/// <summary>
/// User service for handling operations from API
/// </summary>
public interface IUserService
{
    Task<OperationResult<IdentityResult>> Register(RegisterRequest registerRequest);
    Task<OperationResult<User>> Login(string username, string password);
}