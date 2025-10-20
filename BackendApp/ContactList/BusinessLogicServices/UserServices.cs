using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ContactList.Entities;
using ContactList.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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

    public async Task<IdentityResult> Register(string username, string email, string password)
    {
        if (await _repository.GetUserByUsernameAsync(username) != null)
        {
            throw new Exception("User with given name already exists");
        }

        if (await _repository.GetUserByEmailAsync(email) != null)
        {
            throw new Exception("User with given email already exists");
        }

        var user = new User()
        {
            UserName = username,
            Email = email
        };
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<string?> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return null;

        var isValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isValid) return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim(ClaimTypes.Name, user.UserName!) },
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}