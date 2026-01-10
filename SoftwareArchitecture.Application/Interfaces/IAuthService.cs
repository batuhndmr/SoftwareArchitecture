using SoftwareArchitecture.Application.DTOs.Auth;

namespace SoftwareArchitecture.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginDto dto);
}
