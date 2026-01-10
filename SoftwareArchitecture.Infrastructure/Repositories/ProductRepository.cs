using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Infrastructure.Persistence;

namespace SoftwareArchitecture.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
        => await _context.Products
            .Where(p => !p.IsDeleted)
            .ToListAsync();

    public async Task<Product?> GetByIdAsync(int id)
        => await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product product)
    {
        // Soft Delete - Fiziksel olarak silme
        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }
}
