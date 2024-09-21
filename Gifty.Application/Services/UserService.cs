using System.Security.Claims;
using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Application.Services;
using Gifty.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Gifty.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace Gifty.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ITokenService tokenService,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(RegisterDTO registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Birthday = registerDto.Birthday
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
        
        public async Task<ServiceResponse<AppUser>> UpdateProfileAsync(UpdateUserProfileDTO dto)
        {
            // Get the current user ID from the token
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return ServiceResponse<AppUser>.FailureResponse("User not authenticated.");
            }

            // Find the user in the database
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return ServiceResponse<AppUser>.FailureResponse("User not found.");
            }

            // Update fields
            user.FullName = dto.FullName ?? user.FullName;
            user.Birthday = dto.Birthday ?? user.Birthday;
            user.Bio = dto.Bio ?? user.Bio;
            user.Location = dto.Location ?? user.Location;
            user.ProfilePictureUrl = dto.ProfilePictureUrl ?? user.ProfilePictureUrl;

            // Save the changes
            await _userRepository.UpdateAsync(user);

            return ServiceResponse<AppUser>.SuccessResponse(user, "Profile updated successfully.");
        }

        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }
    }
}
