using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Application.Services;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IHealthStatusRepository, HealthStatusRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
