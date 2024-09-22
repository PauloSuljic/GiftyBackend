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
    }
}