using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IUserRepository _userRepository;

        public FriendService(IFriendRequestRepository friendRequestRepository, IUserRepository userRepository)
        {
            _friendRequestRepository = friendRequestRepository;
            _userRepository = userRepository;
        }

        // Get all friends for a specific user based on accepted requests
        public async Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsForUserAsync(string userId)
        {
            // Step 1: Get all confirmed friend requests for the logged-in user
            var friendRequests = await _friendRequestRepository.GetConfirmedRequestsForUserAsync(userId);

            // Step 2: Create a list to hold friend DTOs
            var friendDtos = new List<FriendDTO>();

            // Step 3: Loop through each friend request to determine the other user (friend)
            foreach (var friendRequest in friendRequests)
            {
                // Determine the friend's userId (the other user)
                var friendUserId = friendRequest.SenderId == userId ? friendRequest.ReceiverId : friendRequest.SenderId;

                // Step 4: Fetch additional details about the friend from the user repository
                var friend = await _userRepository.GetByIdAsync(friendUserId);

                if (friend != null)
                {
                    // Step 5: Map the friend data to a FriendDTO and add it to the list
                    var friendDto = new FriendDTO
                    {
                        Id = friendRequest.Id,
                        UserId = userId,
                        FriendId = friendUserId,
                        CreatedAt = friendRequest.CreatedAt,
                        Status = friendRequest.Status,
                        FriendName = friend.FullName,
                        FriendDOB = friend.Birthday, // Assuming this is a property in the User entity
                        FriendImage = friend.ProfilePictureUrl // Assuming this is a property in the User entity
                    };
        
                    friendDtos.Add(friendDto);
                }
            }

            // Step 6: Return the list of friend DTOs
            return ServiceResponse<IEnumerable<FriendDTO>>.SuccessResponse(friendDtos, "Friends retrieved successfully.");
        }


        // Send a friend request
        public async Task<ServiceResponse<FriendRequestDTO>> SendRequestAsync(string senderId, string receiverId)
        {
            var existingRequest = await _friendRequestRepository.GetRequestBetweenUsersAsync(senderId, receiverId);
            if (existingRequest != null)
            {
                return ServiceResponse<FriendRequestDTO>.FailureResponse("Request already exists.");
            }

            var request = new FriendRequest
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Status = RequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _friendRequestRepository.AddRequestAsync(request);

            var requestDto = new FriendRequestDTO
            {
                Id = request.Id,
                SenderId = request.SenderId,
                ReceiverId = request.ReceiverId,
                Status = request.Status,
                CreatedAt = request.CreatedAt
            };

            return ServiceResponse<FriendRequestDTO>.SuccessResponse(requestDto, "Friend request sent successfully.");
        }

        // Respond to a friend request
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

        // Remove a friend
        public async Task<ServiceResponse<string>> RemoveFriendAsync(string userId, string friendId)
        {
            var friendRequest = await _friendRequestRepository.GetAcceptedRequestBetweenUsersAsync(userId, friendId);
            if (friendRequest == null)
            {
                return ServiceResponse<string>.FailureResponse("Friend not found.");
            }

            await _friendRequestRepository.DeleteRequestAsync(friendRequest);
            return ServiceResponse<string>.SuccessResponse("Friend removed successfully.");
        }
    }
}
