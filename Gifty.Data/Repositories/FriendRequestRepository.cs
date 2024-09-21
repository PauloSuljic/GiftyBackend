using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gifty.Data.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly AppDbContext _context;

    public FriendRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FriendRequest> GetRequestAsync(int requestId)
    {
        return await _context.FriendRequests.FindAsync(requestId);
    }

    public async Task<IEnumerable<FriendRequest>> GetPendingRequestsForUserAsync(string userId)
    {
        return await _context.FriendRequests
            .Where(r => r.ReceiverId == userId && r.Status == RequestStatus.Pending)
            .ToListAsync();
    }

    public async Task AddRequestAsync(FriendRequest request)
    {
        await _context.FriendRequests.AddAsync(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRequestAsync(FriendRequest request)
    {
        _context.FriendRequests.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRequestAsync(FriendRequest request)
    {
        _context.FriendRequests.Remove(request);
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<AppUser>> GetFriendsByUserIdAsync(string userId)
    {
        // Get the accepted friend requests where the user is either the sender or receiver
        var friendRequests = await _context.FriendRequests
            .Where(fr => (fr.SenderId == userId || fr.ReceiverId == userId) && fr.Status == RequestStatus.Accepted)
            .ToListAsync();

        // Extract the friend IDs
        var friendIds = friendRequests
            .Select(fr => fr.SenderId == userId ? fr.ReceiverId : fr.SenderId)
            .ToList();

        // Retrieve the users based on the friend IDs
        return await _context.Users
            .Where(u => friendIds.Contains(u.Id))
            .ToListAsync();
    }

}
