using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public interface IFriendService
    {
        Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsForUserAsync(string userId);
        Task<ServiceResponse<string>> RemoveFriendAsync(string userId, string friendId);
        Task<ServiceResponse<FriendRequestDTO>> SendRequestAsync(string senderId, string receiverId);
        Task<ServiceResponse<string>> RespondToRequestAsync(int requestId, bool accept);

    }
}