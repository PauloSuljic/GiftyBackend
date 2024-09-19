using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Application.Services;
using Gifty.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(RegisterDTO registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return ServiceResponse<string>.FailureResponse("User registration failed.");
            }

            var token = _tokenService.CreateToken(user);
            return ServiceResponse<string>.SuccessResponse(token, "User registered successfully.");
        }

        public async Task<ServiceResponse<string>> LoginAsync(LoginDTO loginDto)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return ServiceResponse<string>.FailureResponse("Invalid email.");
            }

            // Check if the password matches the user's password
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return ServiceResponse<string>.FailureResponse("Invalid password.");
            }

            // Generate a token for the user
            var token = _tokenService.CreateToken(user);
            return ServiceResponse<string>.SuccessResponse(token, "User logged in successfully.");
        }


        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
