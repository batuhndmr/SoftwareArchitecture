using System.ComponentModel.DataAnnotations;

namespace SoftwareArchitecture.Application.DTOs.Users
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
