using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.DTOs.Products;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Api.Models;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Tüm endpoint'ler yetkilendirilmiş
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET api/products
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all products");
        var products = await _productService.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(products, "Products fetched successfully"));
    }

    // GET api/products/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting product with id: {ProductId}", id);
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product with id {ProductId} not found", id);
            return NotFound(ApiResponse<object>.Fail("Product not found"));
        }

        return Ok(ApiResponse<object>.Ok(product, "Product fetched successfully"));
    }

    // POST api/products
    [HttpPost]
    [Authorize(Roles = "Admin")] // Sadece Admin ürün oluşturabilir
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        _logger.LogInformation("Creating new product: {ProductName}", dto.Name);
        var product = await _productService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById), 
            new { id = product.Id }, 
            ApiResponse<object>.Ok(product, "Product created successfully"));
    }

    // PUT api/products/{id}
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")] // Sadece Admin ürün güncelleyebilir
    public async Task<IActionResult> Update(int id, [FromBody] CreateProductDto dto)
    {
        _logger.LogInformation("Updating product with id: {ProductId}", id);
        var product = await _productService.UpdateAsync(id, dto);
        if (product == null)
        {
            _logger.LogWarning("Product with id {ProductId} not found for update", id);
            return NotFound(ApiResponse<object>.Fail("Product not found"));
        }

        return Ok(ApiResponse<object>.Ok(product, "Product updated successfully"));
    }

    // DELETE api/products/{id}
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] // Sadece Admin ürün silebilir
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting product with id: {ProductId}", id);
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Product with id {ProductId} not found for deletion", id);
            return NotFound(ApiResponse<object>.Fail("Product not found"));
        }

        return NoContent();
    }
}
