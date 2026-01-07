using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftwareArchitecture.Application.DTOs.Users;
using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<UserResponseDto>> GetUsersAsync()
        {
            var users = await _repository.GetAllAsync();

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name
            }).ToList();
        }

        public async Task<UserResponseDto> CreateAsync(UserCreateDto dto)
        {
            var user = new User
            {
                Name = dto.Name
            };

            await _repository.AddAsync(user);

            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}