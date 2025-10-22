using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ContactList.Entities;
using Microsoft.IdentityModel.Tokens;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJwtToken(string username, DateTime expirationDate)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: new[] { new Claim(ClaimTypes.Name, username) },
            expires: expirationDate,
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public JwtSecurityToken ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        }, out SecurityToken validatedToken);

        return (JwtSecurityToken)validatedToken;
    }

    public OperationResult<string> RefreshToken(string cookie)
    {
        try
        {
            var jwt = this.ValidateJwtToken(cookie);
            var username = jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            var newAccessToken = this.GenerateJwtToken(username, DateTime.UtcNow.AddMinutes(15));
            return OperationResult<string>.Successful(newAccessToken);
        }
        catch
        {
            return OperationResult<string>.UnauthorizedResult();
        }
    }
}