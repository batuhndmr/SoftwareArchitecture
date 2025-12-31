using SoftwareArchitecture.Domain.Entities;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task AddAsync(User user);
}
