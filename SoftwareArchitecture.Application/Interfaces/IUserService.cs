using SoftwareArchitecture.Domain.Entities;


namespace SoftwareArchitecture.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task CreateAsync(string name);
    }
}
