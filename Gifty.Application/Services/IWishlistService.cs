using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Application.Services
{
    public interface IWishlistService
    {
        Task<ServiceResponse<IEnumerable<WishlistDTO>>> GetWishlistsByUserIdAsync(string userId);
        Task<ServiceResponse<WishlistDTO>> CreateWishlistAsync(CreateWishlistDTO wishlistDto);
        Task<ServiceResponse<WishlistDTO>> UpdateWishlistAsync(int wishlistId, EditWishlistDTO wishlistDto);
        Task<ServiceResponse<string>> DeleteWishlistAsync(int wishlistId);
    }
}