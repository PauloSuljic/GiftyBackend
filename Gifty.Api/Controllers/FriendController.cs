using Microsoft.AspNetCore.Mvc;
using Gifty.Application.DTOs;
using Gifty.Application.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Gifty.API.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        // Get all friends for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            var userId = User.FindFirst("userId")?.Value; // Extract user ID from the token
            var response = await _friendService.GetFriendsForUserAsync(userId);
            return Ok(response);
        }

        // Get a specific friend by ID for the logged-in user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriend(int id)
        {
            var userId = User.FindFirst("userId")?.Value; // Extract user ID from the token
            var response = await _friendService.GetFriendByIdAsync(userId, id);
            return Ok(response);
        }

        // Remove a friend for the logged-in user
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFriend(int id)
        {
            var userId = User.FindFirst("userId")?.Value; // Extract user ID from the token
            var response = await _friendService.RemoveFriendAsync(userId, id);
            if (!response.Success)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
    }
}
