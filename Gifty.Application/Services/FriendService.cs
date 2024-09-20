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
        private readonly IFriendRepository _friendRepository;

        public FriendService(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        // Get all friends for a specific user
        public async Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsForUserAsync(string userId)
        {
            var friends = await _friendRepository.GetFriendsByUserIdAsync(userId);
            var friendDtos = friends.Select(f => new FriendDTO
            {
                Id = f.Id,
                UserId = f.UserId,
                FriendId = f.FriendId,
                CreatedAt = f.CreatedAt,
                Status = f.Status
            }).ToList();

            return ServiceResponse<IEnumerable<FriendDTO>>.SuccessResponse(friendDtos, "Friends retrieved successfully.");
        }

        // Get a specific friend by ID for a specific user
        public async Task<ServiceResponse<FriendDTO>> GetFriendByIdAsync(string userId, int friendId)
        {
            var friend = await _friendRepository.GetFriendByIdAndUserIdAsync(friendId, userId);
            if (friend == null)
            {
                return ServiceResponse<FriendDTO>.FailureResponse("Friend not found.");
            }

            var friendDto = new FriendDTO
            {
                Id = friend.Id,
                UserId = friend.UserId,
                FriendId = friend.FriendId,
                CreatedAt = friend.CreatedAt,
                Status = friend.Status
            };

            return ServiceResponse<FriendDTO>.SuccessResponse(friendDto, "Friend retrieved successfully.");
        }

        // Remove a friend for a specific user
        public async Task<ServiceResponse<string>> RemoveFriendAsync(string userId, int friendId)
        {
            var friend = await _friendRepository.GetFriendByIdAndUserIdAsync(friendId, userId);
            if (friend == null)
            {
                return ServiceResponse<string>.FailureResponse("Friend not found.");
            }

            await _friendRepository.DeleteAsync(friend);
            return ServiceResponse<string>.SuccessResponse("Friend removed successfully.");
        }
    }
}
