using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifty.Data.Repositories
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly AppDbContext _context;

        public FriendRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get a friend request by ID
        public async Task<FriendRequest> GetRequestAsync(int requestId)
        {
            return await _context.FriendRequests.FindAsync(requestId);
        }

        // Get all pending friend requests for a user
        public async Task<IEnumerable<FriendRequest>> GetPendingRequestsForUserAsync(string userId)
        {
            return await _context.FriendRequests
                .Where(r => r.ReceiverId == userId && r.Status == RequestStatus.Pending)
                .ToListAsync();
        }

        // Add a new friend request
        public async Task AddRequestAsync(FriendRequest request)
        {
            await _context.FriendRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        // Update an existing friend request
        public async Task UpdateRequestAsync(FriendRequest request)
        {
            _context.FriendRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        // Delete a friend request
        public async Task DeleteRequestAsync(FriendRequest request)
        {
            _context.FriendRequests.Remove(request);
            await _context.SaveChangesAsync();
        }

        // Get confirmed friend requests for a user (Accepted status)
        public async Task<IEnumerable<FriendRequest>> GetConfirmedRequestsForUserAsync(string userId)
        {
            return await _context.FriendRequests
                .Where(fr => (fr.SenderId == userId || fr.ReceiverId == userId) && fr.Status == RequestStatus.Accepted)
                .ToListAsync();
        }

        // Get a request between two users
        public async Task<FriendRequest> GetRequestBetweenUsersAsync(string userId1, string userId2)
        {
            return await _context.FriendRequests
                .FirstOrDefaultAsync(fr => 
                    (fr.SenderId == userId1 && fr.ReceiverId == userId2) || 
                    (fr.SenderId == userId2 && fr.ReceiverId == userId1));
        }

        // Get an accepted request between two users (for checking if they are already friends)
        public async Task<FriendRequest> GetAcceptedRequestBetweenUsersAsync(string userId1, string userId2)
        {
            return await _context.FriendRequests
                .FirstOrDefaultAsync(fr => 
                    ((fr.SenderId == userId1 && fr.ReceiverId == userId2) || 
                     (fr.SenderId == userId2 && fr.ReceiverId == userId1)) &&
                     fr.Status == RequestStatus.Accepted);
        }
    }
}
