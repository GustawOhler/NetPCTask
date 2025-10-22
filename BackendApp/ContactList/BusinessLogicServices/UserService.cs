using ContactList.DTOs;
using ContactList.Entities;
using ContactList.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ContactList.BusinessLogicServices;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _config;
    private readonly UserManager<User> _userManager;

    public UserService(IUserRepository repository, IConfiguration config, UserManager<User> userManager)
    {
        this._repository = repository;
        this._config = config;
        this._userManager = userManager;
    }

    public async Task<OperationResult<IdentityResult>> Register(RegisterRequest registerRequest)
    {
        if (await _repository.GetUserByUsernameAsync(registerRequest.UserName) != null)
        {
            return OperationResult<IdentityResult>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(registerRequest.UserName), new[] { "User with given username already exists" } }
            });
        }

        if (await _repository.GetUserByEmailAsync(registerRequest.Email) != null)
        {
            return OperationResult<IdentityResult>.ValidationFailed(new Dictionary<string, string[]>
            {
                { nameof(registerRequest.Email), new[] { "User with given email already exists" } }
            });
        }

        var user = new User()
        {
            UserName = registerRequest.UserName,
            Email = registerRequest.Email
        };
        var result = await _userManager.CreateAsync(user, registerRequest.Password);

        if (result.Succeeded)
        {
            return OperationResult<IdentityResult>.Successful(result);
        }
        
        return OperationResult<IdentityResult>.ValidationFailed(new Dictionary<string, string[]>
            {
                { "UserDetails", result.Errors.Select(e => e.Description).ToArray()}
            });
    }

    public async Task<OperationResult<User>> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return OperationResult<User>.NotFoundResult();

        var isValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isValid) return OperationResult<User>.UnauthorizedResult();

        return OperationResult<User>.Successful(user);
    }
}