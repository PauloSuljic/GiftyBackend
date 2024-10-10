using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Application.Services;
using Gifty.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gifty.Data.Repositories;
using Microsoft.AspNetCore.Http;

namespace Gifty.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistService(IWishlistRepository wishlistRepository, IHttpContextAccessor httpContextAccessor)
        {
            _wishlistRepository = wishlistRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse<IEnumerable<WishlistDTO>>> GetWishlistsByUserIdAsync(string userId)
        {
            var wishlists = await _wishlistRepository.GetByUserIdAsync(userId); // Adjust your repository method to fetch by userId
            var wishlistDtos = wishlists.Select(w => new WishlistDTO
            {
                Id = w.Id,
                Title = w.Title
            }).ToList();

            return ServiceResponse<IEnumerable<WishlistDTO>>.SuccessResponse(wishlistDtos, "Wishlists retrieved successfully.");
        }

        public async Task<ServiceResponse<WishlistDTO>> GetWishlistByIdAsync(int id)
        {
            // Fetch the wishlist from the repository by its ID
            var wishlist = await _wishlistRepository.GetByIdAsync(id);

            // Check if the wishlist exists
            if (wishlist == null)
            {
                return ServiceResponse<WishlistDTO>.FailureResponse("Wishlist not found.");
            }

            // Map the wishlist entity to WishlistDTO
            var wishlistDto = new WishlistDTO
            {
                Id = wishlist.Id,
                Title = wishlist.Title
            };

            return ServiceResponse<WishlistDTO>.SuccessResponse(wishlistDto, "Wishlist retrieved successfully.");
        }



        public async Task<ServiceResponse<WishlistDTO>> CreateWishlistAsync(CreateWishlistDTO wishlistDto)
        {
            // Extract the user ID from the JWT token
            var userId = _httpContextAccessor.HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return ServiceResponse<WishlistDTO>.FailureResponse("User ID not found.");
            }

            var wishlist = new Wishlist
            {
                Title = wishlistDto.Title,
                AppUserId = userId // Associate the wishlist with the authenticated user
            };

            await _wishlistRepository.AddAsync(wishlist);
        
            return ServiceResponse<WishlistDTO>.SuccessResponse(new WishlistDTO
            {
                Id = wishlist.Id,
                Title = wishlist.Title
            }, "Wishlist created successfully.");
        }

        public async Task<ServiceResponse<WishlistDTO>> UpdateWishlistAsync(int wishlistId, EditWishlistDTO wishlistDto)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(wishlistId);
            if (wishlist == null)
            {
                return ServiceResponse<WishlistDTO>.FailureResponse("Wishlist not found.");
            }

            wishlist.Title = wishlistDto.Title;
            await _wishlistRepository.UpdateAsync(wishlist);

            return ServiceResponse<WishlistDTO>.SuccessResponse(new WishlistDTO
            {
                Id = wishlist.Id,
                Title = wishlist.Title
            }, "Wishlist updated successfully.");
        }

        public async Task<ServiceResponse<string>> DeleteWishlistAsync(int wishlistId)
        {
            var wishlist = await _wishlistRepository.GetByIdAsync(wishlistId);
            if (wishlist == null)
            {
                return ServiceResponse<string>.FailureResponse("Wishlist not found.");
            }

            await _wishlistRepository.DeleteAsync(wishlist);
            return ServiceResponse<string>.SuccessResponse("Wishlist deleted successfully.");
        }
    }
}
