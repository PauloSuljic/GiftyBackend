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
}
