using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public interface IFriendService
    {
        Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsForUserAsync(string userId);
        Task<ServiceResponse<FriendDTO>> GetFriendByIdAsync(string userId, int friendId);
        Task<ServiceResponse<string>> RemoveFriendAsync(string userId, int friendId);
    }
}