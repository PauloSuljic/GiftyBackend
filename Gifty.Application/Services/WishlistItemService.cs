using Gifty.Application.DTOs;
using Gifty.Application.Responses;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using Gifty.Domain.Services;

namespace Gifty.Application.Services
{
    public class WishlistItemService : IWishlistItemService
    {
        private readonly IWishlistItemRepository _wishlistItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WishlistItemService(IWishlistItemRepository wishlistItemRepository, IUnitOfWork unitOfWork)
        {
            _wishlistItemRepository = wishlistItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse<WishlistItemDTO>> AddItemAsync(int wishlistId, CreateWishlistItemDTO itemDto)
        {
            var wishlistItem = new WishlistItem
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price,
                Link = itemDto.Link,
                WishlistId = wishlistId
            };

            await _wishlistItemRepository.AddAsync(wishlistItem);
            await _unitOfWork.SaveChangesAsync();

            var wishlistItemDto = new WishlistItemDTO
            {
                Id = wishlistItem.Id,
                Name = wishlistItem.Name,
                Description = wishlistItem.Description,
                Price = wishlistItem.Price,
                WishlistId = wishlistItem.WishlistId
            };

            return ServiceResponse<WishlistItemDTO>.SuccessResponse(wishlistItemDto, "Wishlist item added successfully.");
        }

        public async Task<ServiceResponse<WishlistItemDTO>> UpdateItemAsync(int itemId, UpdateWishlistItemDTO itemDto)
        {
            var item = await _wishlistItemRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return ServiceResponse<WishlistItemDTO>.FailureResponse("Item not found.");

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.Price = itemDto.Price;

            await _unitOfWork.SaveChangesAsync();

            var updatedItemDto = new WishlistItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                WishlistId = item.WishlistId
            };

            return ServiceResponse<WishlistItemDTO>.SuccessResponse(updatedItemDto, "Wishlist item updated successfully.");
        }

        public async Task<ServiceResponse<bool>> RemoveItemAsync(int itemId)
        {
            var item = await _wishlistItemRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return ServiceResponse<bool>.FailureResponse("Item not found.");

            _wishlistItemRepository.RemoveByIdAsync(item.Id);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResponse<bool>.SuccessResponse(true, "Wishlist item removed successfully.");
        }

        public async Task<ServiceResponse<IEnumerable<WishlistItemDTO>>> GetItemsForWishlistAsync(int wishlistId)
        {
            var items = await _wishlistItemRepository.GetItemsByWishlistIdAsync(wishlistId);

            var itemDtos = items.Select(item => new WishlistItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                WishlistId = item.WishlistId
            }).ToList();

            return ServiceResponse<IEnumerable<WishlistItemDTO>>.SuccessResponse(itemDtos, "Wishlist items retrieved successfully.");
        }

        public async Task<ServiceResponse<WishlistItemDTO>> GetItemByIdAsync(int itemId)
        {
            var item = await _wishlistItemRepository.GetItemByIdAsync(itemId);
            if (item == null)
                return ServiceResponse<WishlistItemDTO>.FailureResponse("Item not found.");

            var itemDto = new WishlistItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                WishlistId = item.WishlistId
            };

            return ServiceResponse<WishlistItemDTO>.SuccessResponse(itemDto, "Wishlist item retrieved successfully.");
        }
    }
}
