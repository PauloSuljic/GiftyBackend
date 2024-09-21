using Microsoft.EntityFrameworkCore;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;

namespace Gifty.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetByIdAsync(string userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
        
    }
}