using Microsoft.AspNetCore.Mvc;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Gifty.API.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Get all users in the Gifty app
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return Ok(response);
        }
    }
}
