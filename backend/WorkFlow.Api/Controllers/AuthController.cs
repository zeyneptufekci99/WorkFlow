using Microsoft.AspNetCore.Mvc;
using WorkFlow.Api.DTOs.Auth;
using WorkFlow.Api.Services;

namespace WorkFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var token = await _authService.RegisterAsync(request);

        if (token is null)
        {
            return Conflict(new
            {
                message = "A user with this email already exists."
            });
        }

        return Ok(new
        {
            token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(request);

        if (token is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(new
        {
            token
        });
    }
}