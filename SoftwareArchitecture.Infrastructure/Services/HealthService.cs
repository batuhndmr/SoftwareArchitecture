using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Entities;

namespace SoftwareArchitecture.Infrastructure.Services
{
    public class HealthService : IHealthService
    {
        public HealthStatus Check()
        {
            return new HealthStatus
                {
                IsHealthy = true,
                Message = "API + Architecture ayakta !!!"
            };
        }
    }
}
