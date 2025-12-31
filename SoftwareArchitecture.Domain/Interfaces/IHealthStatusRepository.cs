namespace SoftwareArchitecture.Domain.Interfaces;

using SoftwareArchitecture.Domain.Entities;

public interface IHealthStatusRepository
{
    HealthStatus GetStatus();
}
