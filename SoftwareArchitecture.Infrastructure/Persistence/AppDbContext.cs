using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Domain.Entities;

namespace SoftwareArchitecture.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<HealthStatus> HealthStatuses => Set<HealthStatus>();
}
