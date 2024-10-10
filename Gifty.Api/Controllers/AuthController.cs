using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Application.Services;
using Gifty.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Gifty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly IUserService _userService;

        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            EmailService emailService,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _userService = userService;
        }

        // Register a new user
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                Birthday = registerDto.Birthday,
                Bio = string.Empty,
                Location = string.Empty,
                ProfilePictureUrl = string.Empty
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(ServiceResponse<string>.FailureResponse("User registration failed."));
            }

            // Generate email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Create a verification link (use your backend domain if no frontend)
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new
            {
                userId = user.Id,
                token = token
            }, Request.Scheme);

            // Send verification email
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking the following link: {confirmationLink}");

            return Ok(ServiceResponse<string>.SuccessResponse("Registration successful. Please confirm your email."));
        }

        // Confirm the email using the token sent to the user's email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest("Invalid confirmation parameters.");

            // Find the user
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            // Confirm the email using the token
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully!");

            return BadRequest("Email confirmation failed.");
        }

        // User login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(ServiceResponse<string>.FailureResponse("Invalid email."));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized(ServiceResponse<string>.FailureResponse("Invalid password."));
            }

            // Check if the email is confirmed
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(ServiceResponse<string>.FailureResponse("Please confirm your email before logging in."));
            }

            // Generate JWT token
            var token = _tokenService.CreateToken(user);
            return Ok(ServiceResponse<string>.SuccessResponse(token, "Login successful."));
        }

        // Resend email confirmation
        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (await _userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Email already confirmed.");
            }

            // Generate a new email confirmation token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Create the confirmation link
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new
            {
                userId = user.Id,
                token = token
            }, Request.Scheme);

            // Send confirmation email
            await _emailService.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your email by clicking the following link: {confirmationLink}");

            return Ok("Confirmation email sent.");
        }

        // Forgot password (optional feature)
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Generate password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create reset password link (same as confirmation process)
            var resetLink = Url.Action(nameof(ResetPassword), "Auth", new
            {
                userId = user.Id,
                token = resetToken
            }, Request.Scheme);

            // Send password reset email
            await _emailService.SendEmailAsync(user.Email, "Reset your password",
                $"Click the following link to reset your password: {resetLink}");

            return Ok("Password reset email sent.");
        }

        // Reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
    
            // Verify the token
            var isTokenValid = await _userService.VerifyUserTokenAsync(user, dto.Token, "ResetPassword");
            if (!isTokenValid)
            {
                return BadRequest("Invalid token.");
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (result.Succeeded)
            {
                return Ok("Password reset successful.");
            }

            return BadRequest("Password reset failed.");
        }
    }
}
