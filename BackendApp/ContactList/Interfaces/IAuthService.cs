using System.IdentityModel.Tokens.Jwt;

/// <summary>
/// Auth service interface for managing JWT
/// </summary>
public interface IAuthService
{
    public string GenerateJwtToken(string username, DateTime expirationDate);
    public JwtSecurityToken ValidateJwtToken(string token);
    public OperationResult<string> RefreshToken(string cookie);
}