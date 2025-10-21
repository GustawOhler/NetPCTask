using ContactList.DTOs;
using ContactList.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IConfiguration config, IUserService userService, IAuthService authService)
    {
        _config = config;
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (!MiniValidator.TryValidate(req, out var errors))
        {
            // return ValidationProblem(errors);
            return ValidationProblem();
        }

        var user = await _userService.Login(req.UserName, req.Password);

        if (user == null) return Unauthorized();

        var accessToken = _authService.GenerateJwtToken(user.UserName!, DateTime.UtcNow.AddMinutes(15));
        var refreshToken = _authService.GenerateJwtToken(user.UserName!, DateTime.UtcNow.AddDays(7)); // 7 days

        Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7),
            Path = "/api/auth/refresh"
        });

        return Ok(new { accessToken });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        var cookie = Request.Cookies["refreshToken"];
        if (cookie == null)
            return Unauthorized();

        try
        {
            var jwt = _authService.ValidateJwtToken(cookie);
            var username = jwt.Claims.First(c => c.Type == ClaimTypes.Name).Value;

            var newAccessToken = _authService.GenerateJwtToken(username, DateTime.UtcNow.AddMinutes(15));
            return Ok(new { accessToken = newAccessToken });
        }
        catch
        {
            return Unauthorized();
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (!MiniValidator.TryValidate(req, out var errors))
        {
            return ValidationProblem();
            // Add errors
        }

        var result = await _userService.Register(req.UserName, req.Email, req.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}

public record LoginDto(string Username, string Password);
