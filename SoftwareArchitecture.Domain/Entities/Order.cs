namespace SoftwareArchitecture.Domain.Entities;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false; // Soft Delete

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
