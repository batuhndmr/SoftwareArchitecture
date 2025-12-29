using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.Interfaces;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;

    public HealthController(IHealthService healthService)
    {
        _healthService = healthService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var result = _healthService.Check();
        return Ok(result);
    }
}