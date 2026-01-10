using System.Collections.Generic;

namespace SoftwareArchitecture.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false; // Soft Delete

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
