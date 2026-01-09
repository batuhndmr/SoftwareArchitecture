using SoftwareArchitecture.Application.DTOs.Orders;

namespace SoftwareArchitecture.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto?> GetByIdAsync(int id);
    Task<OrderResponseDto> CreateAsync(OrderCreateDto dto);
    Task<OrderResponseDto?> UpdateAsync(int id, OrderCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
