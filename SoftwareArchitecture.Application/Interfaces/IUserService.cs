using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwareArchitecture.Application.DTOs.Users;

namespace SoftwareArchitecture.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetUsersAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto> CreateAsync(UserCreateDto dto);
        Task<UserResponseDto?> UpdateAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
