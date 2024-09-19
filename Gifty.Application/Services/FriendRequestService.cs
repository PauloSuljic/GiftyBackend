using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;

namespace Gifty.Application.Services
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;

        public FriendRequestService(IFriendRequestRepository friendRequestRepository)
        {
            _friendRequestRepository = friendRequestRepository;
        }

        public async Task<ServiceResponse<FriendRequestDTO>> SendRequestAsync(string senderId, SendFriendRequestDTO requestDto)
        {
            var request = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = requestDto.ReceiverId,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _friendRequestRepository.AddRequestAsync(request);

            var requestDtoResponse = new FriendRequestDTO
            {
                Id = request.Id,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return ServiceResponse<FriendRequestDTO>.SuccessResponse(requestDtoResponse, "Friend request sent successfully.");
        }

        public async Task<ServiceResponse<string>> RespondToRequestAsync(int requestId, bool accept)
        {
            var request = await _friendRequestRepository.GetRequestAsync(requestId);
            if (request == null)
            {
                return ServiceResponse<string>.FailureResponse("Friend request not found.");
            }

            request.Status = accept ? RequestStatus.Accepted : RequestStatus.Declined;
            await _friendRequestRepository.UpdateRequestAsync(request);

            return ServiceResponse<string>.SuccessResponse(accept ? "Friend request accepted." : "Friend request declined.");
        }

        public async Task<ServiceResponse<IEnumerable<FriendRequestDTO>>> GetPendingRequestsAsync(string userId)
        {
            var requests = await _friendRequestRepository.GetPendingRequestsForUserAsync(userId);
            var requestDtos = requests.Select(r => new FriendRequestDTO
            {
                Id = r.Id,
                SenderId = r.SenderId,
                ReceiverId = r.ReceiverId,
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList();

            return ServiceResponse<IEnumerable<FriendRequestDTO>>.SuccessResponse(requestDtos, "Pending requests retrieved successfully.");
        }
    }
}
