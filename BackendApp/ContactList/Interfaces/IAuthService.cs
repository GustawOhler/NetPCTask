using System.IdentityModel.Tokens.Jwt;
using ContactList.Entities;

public interface IAuthService
{
    public string GenerateJwtToken(string username, DateTime expirationDate);
    public JwtSecurityToken ValidateJwtToken(string token);
}