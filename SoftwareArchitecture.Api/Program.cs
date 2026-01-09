using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Application.Services;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;
using SoftwareArchitecture.Infrastructure.Repositories;
using SoftwareArchitecture.Api.Middlewares;
using System.Text.Json;
using SoftwareArchitecture.Api.Models;
using SoftwareArchitecture.Application.DTOs.Orders;
using SoftwareArchitecture.Application.DTOs.Products;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            var errorMessage = string.Join(", ", errors);
            var response = ApiResponse<object>.Fail($"Validation failed: {errorMessage}");
            return new BadRequestObjectResult(response);
        };
    });

// SQLITE CONNECTION
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=software_architecture.db"));

// DI's
// UserService DI
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// OrderService DI
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ProductService DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();



// OpenAPI/Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Software Architecture API",
        Version = "v1",
        Description = ".NET 9 REST API - Clean Architecture Implementation"
    });
});

var app = builder.Build();

// Veritabanı tablolarını otomatik oluştur
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Tabloları oluştur (migration olmadan)
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Veritabanı oluşturulurken bir hata oluştu: {Error}", ex.Message);
    }
}

// Swagger UI - Her zaman açık (Development ve Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Software Architecture API v1");
    c.RoutePrefix = string.Empty; // Swagger UI'ı root'ta göster
});

// Routing
app.UseHttpsRedirection();
app.UseRouting();

// Middleware sırası önemli!
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Status code pages - sadece 404 gibi durumlar için
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    // Sadece body boşsa yaz (çakışmaları engeller)
    if (response.HasStarted) return;

    // Swagger endpoint'lerini etkilemesin
    var path = context.HttpContext.Request.Path.Value?.ToLower() ?? "";
    if (path.StartsWith("/swagger") || path.StartsWith("/openapi") || path == "/")
    {
        return;
    }

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

app.UseAuthorization();


// Order Minimal API
app.MapGet("/api/minimal/orders", async (IOrderService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting all orders via Minimal API");
    var orders = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(orders, "Orders fetched successfully"));
});

app.MapGet("/api/minimal/orders/{id:int}", async (int id, IOrderService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting order with id: {OrderId} via Minimal API", id);
    var order = await service.GetByIdAsync(id);
    if (order == null)
    {
        logger.LogWarning("Order with id {OrderId} not found", id);
        return Results.NotFound(ApiResponse<object>.Fail("Order not found"));
    }
    return Results.Ok(ApiResponse<object>.Ok(order, "Order fetched successfully"));
});

app.MapPost("/api/minimal/orders", async (
    OrderCreateDto dto,
    IOrderService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Creating new order for user: {UserId} via Minimal API", dto.UserId);
    var order = await service.CreateAsync(dto);
    return Results.Created($"/api/minimal/orders/{order.Id}",
        ApiResponse<object>.Ok(order, "Order created successfully"));
});

app.MapPut("/api/minimal/orders/{id:int}", async (
    int id,
    OrderCreateDto dto,
    IOrderService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Updating order with id: {OrderId} via Minimal API", id);
    var order = await service.UpdateAsync(id, dto);
    if (order == null)
    {
        logger.LogWarning("Order with id {OrderId} not found for update", id);
        return Results.NotFound(ApiResponse<object>.Fail("Order not found"));
    }
    return Results.Ok(ApiResponse<object>.Ok(order, "Order updated successfully"));
});

app.MapDelete("/api/minimal/orders/{id:int}", async (int id, IOrderService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Deleting order with id: {OrderId} via Minimal API", id);
    var deleted = await service.DeleteAsync(id);
    if (!deleted)
    {
        logger.LogWarning("Order with id {OrderId} not found for deletion", id);
        return Results.NotFound(ApiResponse<object>.Fail("Order not found"));
    }
    return Results.NoContent();
});

// Product Minimal API
app.MapGet("/api/minimal/products", async (IProductService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting all products via Minimal API");
    var products = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(products, "Products fetched successfully"));
});

app.MapGet("/api/minimal/products/{id:int}", async (int id, IProductService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting product with id: {ProductId} via Minimal API", id);
    var product = await service.GetByIdAsync(id);
    if (product == null)
    {
        logger.LogWarning("Product with id {ProductId} not found", id);
        return Results.NotFound(ApiResponse<object>.Fail("Product not found"));
    }
    return Results.Ok(ApiResponse<object>.Ok(product, "Product fetched successfully"));
});

app.MapPost("/api/minimal/products", async (
    CreateProductDto dto,
    IProductService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Creating new product: {ProductName} via Minimal API", dto.Name);
    var product = await service.CreateAsync(dto);
    return Results.Created($"/api/minimal/products/{product.Id}",
        ApiResponse<object>.Ok(product, "Product created successfully"));
});

app.MapPut("/api/minimal/products/{id:int}", async (
    int id,
    CreateProductDto dto,
    IProductService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Updating product with id: {ProductId} via Minimal API", id);
    var product = await service.UpdateAsync(id, dto);
    if (product == null)
    {
        logger.LogWarning("Product with id {ProductId} not found for update", id);
        return Results.NotFound(ApiResponse<object>.Fail("Product not found"));
    }
    return Results.Ok(ApiResponse<object>.Ok(product, "Product updated successfully"));
});

app.MapDelete("/api/minimal/products/{id:int}", async (int id, IProductService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Deleting product with id: {ProductId} via Minimal API", id);
    var deleted = await service.DeleteAsync(id);
    if (!deleted)
    {
        logger.LogWarning("Product with id {ProductId} not found for deletion", id);
        return Results.NotFound(ApiResponse<object>.Fail("Product not found"));
    }
    return Results.NoContent();
});



// Controllers
app.MapControllers();

app.Run();
