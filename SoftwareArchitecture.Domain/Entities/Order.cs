namespace SoftwareArchitecture.Domain.Entities
{
    public class Order : BaseEntity
    {
        public int UserId { get; set; }

        // Navigation
        public User User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}