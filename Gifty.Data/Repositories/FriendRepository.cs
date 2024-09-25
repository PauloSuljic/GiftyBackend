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

        // Get a specific friend by ID and user ID (additional friend details, if needed)
        public async Task<Friend?> GetFriendByIdAndUserIdAsync(int friendId, string userId)
        {
            return await _context.Friends
                .FirstOrDefaultAsync(f => f.Id == friendId && f.UserId == userId);
        }

        // Add a new friend (if using a separate table for storing additional friend information)
        public async Task AddAsync(Friend friend)
        {
            await _context.Friends.AddAsync(friend);
            await _context.SaveChangesAsync();
        }

        // Delete a friend
        public async Task DeleteAsync(Friend friend)
        {
            _context.Friends.Remove(friend);
            await _context.SaveChangesAsync();
        }
    }
}
