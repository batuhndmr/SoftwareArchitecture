using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Application.Services;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;
using SoftwareArchitecture.Infrastructure.Repositories;
using SoftwareArchitecture.Api.Middlewares;
using System.Text.Json;
using SoftwareArchitecture.Api.Models;


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

app.UseMiddleware<ExceptionHandlingMiddleware>();


app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    // Sadece body boşsa yaz (çakışmaları engeller)
    if (response.HasStarted) return;

    response.ContentType = "application/json";

    var statusCode = response.StatusCode;

    string message = statusCode switch
    {
        404 => "Endpoint not found",
        401 => "Unauthorized",
        403 => "Forbidden",
        400 => "Bad request",
        _ => "Request failed"
    };

    var apiResponse = ApiResponse<object>.Fail(message);

    await response.WriteAsync(JsonSerializer.Serialize(apiResponse));
});
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
