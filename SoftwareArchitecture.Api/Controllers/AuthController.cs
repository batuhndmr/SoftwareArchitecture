using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.DTOs.Auth;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Api.Models;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        _logger.LogInformation("Login attempt for username: {Username}", dto.Username);
        
        var result = await _authService.LoginAsync(dto);
        if (result == null)
        {
            _logger.LogWarning("Login failed for username: {Username}", dto.Username);
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password"));
        }

        return Ok(ApiResponse<object>.Ok(result, "Login successful"));
    }
}
