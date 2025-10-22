using ContactList.DTOs;
using ContactList.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MiniValidation;

/// <summary>
/// Thin controller to receive requests for Auth and pass it to Service (handler) layer
/// </summary>
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
            return BadRequest(new
            {
                title = "Validation error",
                status = 400,
                errors = errors
            });
        }

        var result = await _userService.Login(req.UserName, req.Password);

        if (!result.Success)
        {
            if (result.NotFound)
            {
                return NotFound();
            }
            if (result.Unauthorized)
            {
                return Unauthorized();
            }
            return Problem();
        }

        var user = result.Value!;

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

        var result = _authService.RefreshToken(cookie);

        if (result.Unauthorized)
        {
            return Unauthorized();
        }

        return Ok(new { accessToken = result.Value });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (!MiniValidator.TryValidate(req, out var errors))
        {
            return BadRequest(new
            {
                title = "Validation error",
                status = 400,
                errors = errors
            });
        }

        var result = await _userService.Register(req);

        if (result.ValidationErrors != null)
        {
            return BadRequest(new
            {
                title = "Validation error",
                status = 400,
                errors = result.ValidationErrors
            });
        }

        return Ok();
    }
}

public record LoginDto(string Username, string Password);
