using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public interface IFriendService
    {
        Task<ServiceResponse<IEnumerable<FriendDTO>>> GetFriendsAsync();
        Task<ServiceResponse<FriendDTO>> AddFriendAsync(AddFriendDTO friendDto);
        Task<ServiceResponse<string>> RemoveFriendAsync(int friendId);
    }
}