using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using System.Threading.Tasks;
using Gifty.Domain.Models;

namespace Gifty.Application.Services
{
    public interface IUserService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterDTO registerDto); // Returns JWT token on success
        Task<ServiceResponse<string>> LoginAsync(LoginDTO loginDto);          // Returns JWT token on success
        Task<ServiceResponse<AppUser>> UpdateProfileAsync(UpdateUserProfileDTO dto);
        void Logout();
    }
}