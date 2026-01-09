using Microsoft.AspNetCore.Mvc;
using SoftwareArchitecture.Application.DTOs.Orders;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Api.Models;

namespace SoftwareArchitecture.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Getting all orders");
        var orders = await _orderService.GetAllAsync();
        return Ok(ApiResponse<object>.Ok(orders, "Orders fetched successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting order with id: {OrderId}", id);
        var order = await _orderService.GetByIdAsync(id);
        if (order == null)
        {
            _logger.LogWarning("Order with id {OrderId} not found", id);
            return NotFound(ApiResponse<object>.Fail("Order not found"));
        }

        return Ok(ApiResponse<object>.Ok(order, "Order fetched successfully"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
    {
        _logger.LogInformation("Creating new order for user: {UserId}", dto.UserId);
        var order = await _orderService.CreateAsync(dto);
        return CreatedAtAction(
            nameof(GetById), 
            new { id = order.Id }, 
            ApiResponse<object>.Ok(order, "Order created successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] OrderCreateDto dto)
    {
        _logger.LogInformation("Updating order with id: {OrderId}", id);
        var order = await _orderService.UpdateAsync(id, dto);
        if (order == null)
        {
            _logger.LogWarning("Order with id {OrderId} not found for update", id);
            return NotFound(ApiResponse<object>.Fail("Order not found"));
        }

        return Ok(ApiResponse<object>.Ok(order, "Order updated successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting order with id: {OrderId}", id);
        var deleted = await _orderService.DeleteAsync(id);
        if (!deleted)
        {
            _logger.LogWarning("Order with id {OrderId} not found for deletion", id);
            return NotFound(ApiResponse<object>.Fail("Order not found"));
        }

        return NoContent();
    }
}
