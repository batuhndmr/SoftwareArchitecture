using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftwareArchitecture.Application.DTOs.Auth;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null) return null;

        // Basit password kontrolü (production'da hash karşılaştırması yapılmalı)
        var passwordHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(dto.Password));
        if (user.PasswordHash != passwordHash) return null;

        // JWT Token oluştur
        var token = GenerateJwtToken(user);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };
    }

    private string GenerateJwtToken(SoftwareArchitecture.Domain.Entities.User user)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "SoftwareArchitecture",
            audience: _configuration["Jwt:Audience"] ?? "SoftwareArchitecture",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
