using SoftwareArchitecture.Domain.Entities;

namespace SoftwareArchitecture.Application.Interfaces
{
    public interface IHealthService
    {
        HealthStatus Check();
    }
}
