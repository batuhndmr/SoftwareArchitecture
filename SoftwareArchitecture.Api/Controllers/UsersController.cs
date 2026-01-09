using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.DTOs.Users;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Api.Models;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService service, ILogger<UsersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Getting all users");
        var users = await _service.GetUsersAsync();
        return Ok(ApiResponse<object>.Ok(users, "Users fetched successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting user with id: {UserId}", id);
        var user = await _service.GetByIdAsync(id);
        if (user == null)
        {
            _logger.LogWarning("User with id {UserId} not found", id);
            return NotFound(ApiResponse<object>.Fail("User not found"));
        }

        return Ok(ApiResponse<object>.Ok(user, "User fetched successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserCreateDto dto)
    {
        _logger.LogInformation("Creating new user: {UserName}", dto.Name);
        var createdUser = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdUser.Id },
            ApiResponse<object>.Ok(createdUser, "User created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
    {
        _logger.LogInformation("Updating user with id: {UserId}", id);
        var user = await _service.UpdateAsync(id, dto);
        if (user == null)
        {
            _logger.LogWarning("User with id {UserId} not found for update", id);
            return NotFound(ApiResponse<object>.Fail("User not found"));
        }

        return Ok(ApiResponse<object>.Ok(user, "User updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting user with id: {UserId}", id);
        var deleted = await _service.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("User with id {UserId} not found for deletion", id);
            return NotFound(ApiResponse<object>.Fail("User not found"));
        }

        return NoContent();
    }
}
