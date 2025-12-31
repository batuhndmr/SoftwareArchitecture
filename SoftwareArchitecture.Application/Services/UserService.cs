using SoftwareArchitecture.Application.Interfaces;
using SoftwareArchitecture.Domain.Entities;
using SoftwareArchitecture.Domain.Interfaces;

namespace SoftwareArchitecture.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task CreateAsync(string name)
    {
        var user = new User { Name = name };
        await _repository.AddAsync(user);
    }
}
