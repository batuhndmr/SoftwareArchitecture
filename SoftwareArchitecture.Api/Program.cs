using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Application.Services;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;
using SoftwareArchitecture.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// SQLITE CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=software_architecture.db"));

// DI
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IHealthStatusRepository, HealthStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
