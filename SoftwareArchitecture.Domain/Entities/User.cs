namespace SoftwareArchitecture.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty; // JWT Auth için
        public string PasswordHash { get; set; } = string.Empty; // JWT Auth için
        public string Role { get; set; } = "User"; // JWT Auth için: "Admin" veya "User"

        // Navigation
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
