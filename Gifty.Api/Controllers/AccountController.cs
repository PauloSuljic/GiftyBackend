using Gifty.Application.DTOs;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gifty.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            await _userService.RegisterAsync(dto);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var token = await _userService.LoginAsync(dto);
            return Ok(token);
        }
    }
}