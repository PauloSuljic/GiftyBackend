using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gifty.Domain.Models;
using Gifty.Data;
using Gifty.Domain.Repositories;

namespace Gifty.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private readonly AppDbContext _context;

        public FriendRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Friend>> GetFriendsByUserIdAsync(string userId)
        {
            return await _context.Friends
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Friend?> GetFriendByIdAndUserIdAsync(int friendId, string userId)
        {
            return await _context.Friends
                .FirstOrDefaultAsync(f => f.Id == friendId && f.UserId == userId);
        }

        public async Task AddAsync(Friend friend)
        {
            await _context.Friends.AddAsync(friend);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Friend friend)
        {
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }
    }
}