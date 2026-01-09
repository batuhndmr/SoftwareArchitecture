using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Api.Models;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(ApiResponse<object>.Ok("API is running", "Health check successful"));
    }
}
