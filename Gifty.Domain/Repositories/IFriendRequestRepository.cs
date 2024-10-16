using Gifty.Domain.Models;

namespace Gifty.Domain.Repositories;

public interface IFriendRequestRepository
{
    Task<FriendRequest> GetRequestAsync(int requestId);
    Task<IEnumerable<FriendRequest>> GetPendingRequestsForUserAsync(string userId);
    Task AddRequestAsync(FriendRequest request);
    Task UpdateRequestAsync(FriendRequest request);
    Task DeleteRequestAsync(FriendRequest request);
    Task<IEnumerable<FriendRequest>> GetConfirmedRequestsForUserAsync(string userId);
    Task<FriendRequest> GetRequestBetweenUsersAsync(string userId1, string userId2);
    Task<FriendRequest> GetAcceptedRequestBetweenUsersAsync(string userId1, string userId2);
}
