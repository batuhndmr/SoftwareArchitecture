using SoftwareArchitecture.Domain.Interfaces;
using SoftwareArchitecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SoftwareArchitecture.Infrastructure.Persistence;


namespace SoftwareArchitecture.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
