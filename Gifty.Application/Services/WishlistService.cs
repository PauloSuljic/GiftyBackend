using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Application.Services;
using Gifty.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gifty.Data.Repositories;

namespace Gifty.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<ServiceResponse<IEnumerable<WishlistDTO>>> GetWishlistsAsync()
        {
            var wishlists = await _wishlistRepository.GetAllAsync();
            var wishlistDtos = wishlists.Select(w => new WishlistDTO
            {
                Id = w.Id,
                Title = w.Title
            }).ToList();

            return ServiceResponse<IEnumerable<WishlistDTO>>.SuccessResponse(wishlistDtos, "Wishlists retrieved successfully.");
        }

        public async Task<ServiceResponse<WishlistDTO>> CreateWishlistAsync(CreateWishlistDTO wishlistDto)
        {
            var wishlist = new Wishlist
            {
                Title = wishlistDto.Title
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
