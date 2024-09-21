using Gifty.Application.DTOs;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gifty.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{

    private readonly IUserService _userService;

    public UserProfileController(IUserService userService)
    { 
        _userService = userService;
    }
    [HttpPut("update-profile")] 
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileDTO dto)
    { 
        var response = await _userService.UpdateProfileAsync(dto); 
        if (!response.Success) 
        { 
            return BadRequest(response.Message);
        }
        return Ok(response.Message);
    }
}

