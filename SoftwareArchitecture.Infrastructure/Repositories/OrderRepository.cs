using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;

namespace SoftwareArchitecture.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders
            .Where(o => !o.IsDeleted)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Order order)
    {
        // Soft Delete - Fiziksel olarak silme
        order.IsDeleted = true;
        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
