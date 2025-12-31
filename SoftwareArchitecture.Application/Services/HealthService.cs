using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Application.Services;

public class HealthService : IHealthService
{
    private readonly IHealthStatusRepository _repository;

    public HealthService(IHealthStatusRepository repository)
    {
        _repository = repository;
    }

    public HealthStatus Check()
    {
        return _repository.GetStatus();
    }
}
