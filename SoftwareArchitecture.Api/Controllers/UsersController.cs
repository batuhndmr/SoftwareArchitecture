using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.DTOs.Users;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Api.Models;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await _service.GetUsersAsync();

        return Ok(ApiResponse<object>.Ok(users, "Users fetched successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] UserCreateDto dto)
    {
        var createdUser = await _service.CreateAsync(dto);

        return Created(
            $"api/users/{createdUser.Id}",
            ApiResponse<object>.Ok(createdUser, "User created successfully")
        );
    }
}