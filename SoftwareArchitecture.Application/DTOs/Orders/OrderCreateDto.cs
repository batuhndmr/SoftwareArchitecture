using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitecture.Application.DTOs.Orders;

public class OrderCreateDto
{
    [Required(ErrorMessage = "UserId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Items are required")]
    [MinLength(1, ErrorMessage = "At least one item is required")]
    public List<OrderItemCreateDto> Items { get; set; } = new();
}
