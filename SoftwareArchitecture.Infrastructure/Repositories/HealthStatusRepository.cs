using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;

public class HealthStatusRepository : IHealthStatusRepository
{
    private readonly AppDbContext _context;

    public HealthStatusRepository(AppDbContext context)
    {
        _context = context;
    }

    public HealthStatus GetStatus()
    {
        return new HealthStatus
        {
            IsHealthy = true,
            Message = "DB + Clean Architecture ayakta 🚀"
        };
    }
}
