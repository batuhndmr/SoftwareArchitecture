using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.Interfaces;

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
        return Ok(await _service.GetUsersAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string name)
    {
        await _service.CreateAsync(name);
        return Ok();
    }
}
