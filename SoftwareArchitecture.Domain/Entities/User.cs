namespace SoftwareArchitecture.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}