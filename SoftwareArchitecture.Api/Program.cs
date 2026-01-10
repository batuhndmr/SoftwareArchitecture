using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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

// AuthService DI
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!ForJWTTokenGeneration";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SoftwareArchitecture";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SoftwareArchitecture";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOrAdmin", policy => policy.RequireRole("User", "Admin"));
});

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

    // JWT Authentication için Swagger yapılandırması
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter ONLY the token (Bearer prefix will be added automatically)",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Veritabanı tablolarını otomatik oluştur ve Seed Data ekle
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Tabloları oluştur (migration olmadan)

        // Seed Data - Admin ve User kullanıcılarını kontrol et ve ekle
        var adminPasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("admin123"));
        var userPasswordHash = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("user123"));

        // Admin kullanıcısı var mı kontrol et
        var adminExists = context.Users.Any(u => u.Username == "admin" && !u.IsDeleted);
        if (!adminExists)
        {
            var adminUser = new SoftwareArchitecture.Domain.Entities.User
            {
                Name = "Admin User",
                Username = "admin",
                PasswordHash = adminPasswordHash,
                Role = "Admin",
                IsDeleted = false
            };
            context.Users.Add(adminUser);
        }

        // Normal user var mı kontrol et
        var userExists = context.Users.Any(u => u.Username == "user" && !u.IsDeleted);
        if (!userExists)
        {
            var normalUser = new SoftwareArchitecture.Domain.Entities.User
            {
                Name = "Test User",
                Username = "user",
                PasswordHash = userPasswordHash,
                Role = "User",
                IsDeleted = false
            };
            context.Users.Add(normalUser);
        }

        // Değişiklikleri kaydet
        if (!adminExists || !userExists)
        {
            context.SaveChanges();
        }

        // Seed Products - Eğer ürün yoksa ekle
        if (!context.Products.Any())
        {
            var products = new List<SoftwareArchitecture.Domain.Entities.Product>
            {
                new() { Name = "Laptop", Price = 15000.00m, IsDeleted = false },
                new() { Name = "Mouse", Price = 250.50m, IsDeleted = false },
                new() { Name = "Keyboard", Price = 500.00m, IsDeleted = false },
                new() { Name = "Monitor", Price = 3000.00m, IsDeleted = false }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }

        // Seed Orders - Eğer sipariş yoksa ekle
        if (!context.Orders.Any())
        {
            var savedUser = context.Users.FirstOrDefault(u => u.Username == "user" && !u.IsDeleted);
            var savedProducts = context.Products.Where(p => !p.IsDeleted).Take(2).ToList();

            if (savedUser != null && savedProducts.Count >= 2)
            {
                var order = new SoftwareArchitecture.Domain.Entities.Order
                {
                    UserId = savedUser.Id,
                    IsDeleted = false,
                    OrderItems = new List<SoftwareArchitecture.Domain.Entities.OrderItem>
                    {
                        new() { ProductId = savedProducts[0].Id, Quantity = 1 },
                        new() { ProductId = savedProducts[1].Id, Quantity = 2 }
                    }
                };

                context.Orders.Add(order);
                context.SaveChanges();
            }
        }
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

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

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

// Order Minimal API
app.MapGet("/api/minimal/orders", async (IOrderService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting all orders via Minimal API");
    var orders = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(orders, "Orders fetched successfully"));
}).RequireAuthorization();

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
}).RequireAuthorization();

app.MapPost("/api/minimal/orders", async (
    OrderCreateDto dto,
    IOrderService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Creating new order for user: {UserId} via Minimal API", dto.UserId);
    var order = await service.CreateAsync(dto);
    return Results.Created($"/api/minimal/orders/{order.Id}",
        ApiResponse<object>.Ok(order, "Order created successfully"));
}).RequireAuthorization();

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
}).RequireAuthorization();

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
}).RequireAuthorization();

// Product Minimal API
app.MapGet("/api/minimal/products", async (IProductService service, ILogger<Program> logger) =>
{
    logger.LogInformation("Getting all products via Minimal API");
    var products = await service.GetAllAsync();
    return Results.Ok(ApiResponse<object>.Ok(products, "Products fetched successfully"));
}).RequireAuthorization();

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
}).RequireAuthorization();

app.MapPost("/api/minimal/products", async (
    CreateProductDto dto,
    IProductService service,
    ILogger<Program> logger) =>
{
    logger.LogInformation("Creating new product: {ProductName} via Minimal API", dto.Name);
    var product = await service.CreateAsync(dto);
    return Results.Created($"/api/minimal/products/{product.Id}",
        ApiResponse<object>.Ok(product, "Product created successfully"));
}).RequireAuthorization(policy => policy.RequireRole("Admin"));

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
}).RequireAuthorization(policy => policy.RequireRole("Admin"));

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
}).RequireAuthorization(policy => policy.RequireRole("Admin"));



// Controllers
app.MapControllers();

app.Run();
