using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Infrastructure.Repositories;

public class HealthStatusRepository : IHealthStatusRepository
{
    public HealthStatus GetStatus()
    {
        return new HealthStatus
        {
            IsHealthy = true,
            Message = "API + Architecture ayakta !!!"
        };
    }
}
