using Gifty.Application.DTOs;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Gifty.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;

        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendRequest([FromBody] SendFriendRequestDTO requestDto)
        {
            var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value; // Extract user ID from the token
            if (userId == null)
            {
                return Unauthorized("User ID is not found in token.");
            }

            var response = await _friendRequestService.SendRequestAsync(userId, requestDto);
            return Ok(response);
        }

        [HttpPost("respond/{requestId}")]
        public async Task<IActionResult> RespondToRequest(int requestId, [FromBody] bool accept)
        {
            var response = await _friendRequestService.RespondToRequestAsync(requestId, accept);
            return Ok(response);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingRequests()
        {
            var userId = User.FindFirst("userId")?.Value; // Extract user ID from the token
            var response = await _friendRequestService.GetPendingRequestsAsync(userId);
            return Ok(response);
        }
    }
}