using SoftwareArchitecture.Application.DTOs.Products;

namespace SoftwareArchitecture.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, CreateProductDto dto);
    Task<bool> DeleteAsync(int id);
}
