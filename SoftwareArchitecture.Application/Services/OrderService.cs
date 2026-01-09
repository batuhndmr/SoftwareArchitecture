using SoftwareArchitecture.Application.DTOs.Orders;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();

        return orders.Select(MapToDto).ToList();
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        return order == null ? null : MapToDto(order);
    }

    public async Task<OrderResponseDto> CreateAsync(OrderCreateDto dto)
    {
        var order = new Order
        {
            UserId = dto.UserId,
            OrderItems = dto.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };

        await _orderRepository.AddAsync(order);
        return MapToDto(order);
    }

    public async Task<OrderResponseDto?> UpdateAsync(int id, OrderCreateDto dto)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return null;

        order.UserId = dto.UserId;
        order.UpdatedAt = DateTime.UtcNow;

        order.OrderItems.Clear();
        foreach (var item in dto.Items)
        {
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
        }

        await _orderRepository.UpdateAsync(order);
        return MapToDto(order);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order == null) return false;

        await _orderRepository.DeleteAsync(order);
        return true;
    }

    private static OrderResponseDto MapToDto(Order order)
    {
        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            Items = order.OrderItems.Select(i => new OrderItemResponseDto
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity
            }).ToList()
        };
    }
}
