using Microsoft.AspNetCore.Mvc;
using Gifty.Application.DTOs;
using Gifty.Application.Services;

namespace Gifty.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            var friends = await _friendService.GetFriendsAsync();
            return Ok(friends);
        }

        [HttpPost]
        public async Task<IActionResult> AddFriend(AddFriendDTO friendDto)
        {
            var result = await _friendService.AddFriendAsync(friendDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFriend(int id)
        {
            var result = await _friendService.RemoveFriendAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}