namespace SoftwareArchitecture.Application.DTOs.Orders;

public class OrderResponseDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<OrderItemResponseDto> Items { get; set; } = new();
}

public class OrderItemResponseDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
