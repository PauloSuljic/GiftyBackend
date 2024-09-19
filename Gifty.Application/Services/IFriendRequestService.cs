using Gifty.Application.DTOs;
using Gifty.Application.Responses;

namespace Gifty.Application.Services
{
    public interface IFriendRequestService
    {
        Task<ServiceResponse<FriendRequestDTO>> SendRequestAsync(string senderId, SendFriendRequestDTO requestDto);
        Task<ServiceResponse<string>> RespondToRequestAsync(int requestId, bool accept);
        Task<ServiceResponse<IEnumerable<FriendRequestDTO>>> GetPendingRequestsAsync(string userId);
    }
}