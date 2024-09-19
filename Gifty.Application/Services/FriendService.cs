using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Domain.Models;
using Gifty.Data.Repositories;

namespace Gifty.Application.Services
{
    public class FriendService : IFriendService
    {
        private readonly IFriendRepository _friendRepository;

        public FriendService(IFriendRepository friendRepository)
        {
            _friendRepository = friendRepository;
        }

        public async Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsAsync()
        {
            var friends = await _friendRepository.GetAllAsync();
            var friendDtos = friends.Select(f => new FriendDTO
            {
                Id = f.Id,
                FullName = f.Name,
                BirthDate = f.Birthday
            }).ToList();

            return ServiceResponse<IEnumerable<FriendDTO>>.SuccessResponse(friendDtos,
                "Friends retrieved successfully.");
        }

        public async Task<ServiceResponse<FriendDTO>> AddFriendAsync(AddFriendDTO friendDto)
        {
            var friend = new Friend
            {
                Name = friendDto.FullName,
                Birthday = friendDto.BirthDate
            };

            await _friendRepository.AddAsync(friend);
            return ServiceResponse<FriendDTO>.SuccessResponse(new FriendDTO
            {
                Id = friend.Id,
                FullName = friend.Name,
                BirthDate = friend.Birthday
            }, "Friend added successfully.");
        }

        public async Task<ServiceResponse<string>> RemoveFriendAsync(int friendId)
        {
            var friend = await _friendRepository.GetByIdAsync(friendId);
            if (friend == null)
            {
                return ServiceResponse<string>.FailureResponse("Friend not found.");
            }

            await _friendRepository.DeleteAsync(friend);
            return ServiceResponse<string>.SuccessResponse("Friend removed successfully.");
        }
    }
}
