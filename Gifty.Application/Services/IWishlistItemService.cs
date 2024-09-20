using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Domain.Models;

namespace Gifty.Application.Services
{
    public interface IWishlistItemService
    {
        Task<ServiceResponse<WishlistItemDTO>> AddItemAsync(int wishlistId, CreateWishlistItemDTO itemDto);
        Task<ServiceResponse<WishlistItemDTO>> UpdateItemAsync(int itemId, UpdateWishlistItemDTO itemDto);
        Task<ServiceResponse<bool>> RemoveItemAsync(int itemId);
        Task<ServiceResponse<IEnumerable<WishlistItemDTO>>> GetItemsForWishlistAsync(int wishlistId);
        Task<ServiceResponse<WishlistItemDTO>> GetItemByIdAsync(int itemId);
    }
}