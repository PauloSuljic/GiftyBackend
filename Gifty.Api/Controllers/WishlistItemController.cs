using Gifty.Application.DTOs;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gifty.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistItemsController : ControllerBase
    {
        private readonly IWishlistItemService _wishlistItemService;

        public WishlistItemsController(IWishlistItemService wishlistItemService)
        {
            _wishlistItemService = wishlistItemService;
        }

        [HttpPost("{wishlistId}")]
        public async Task<IActionResult> AddItem(int wishlistId, [FromBody] CreateWishlistItemDTO itemDto)
        {
            var response = await _wishlistItemService.AddItemAsync(wishlistId, itemDto);
            return Ok(response);
        }

        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] UpdateWishlistItemDTO itemDto)
        {
            var response = await _wishlistItemService.UpdateItemAsync(itemId, itemDto);
            return Ok(response);
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> RemoveItem(int itemId)
        {
            var response = await _wishlistItemService.RemoveItemAsync(itemId);
            return Ok(response);
        }

        [HttpGet("{wishlistId}")]
        public async Task<IActionResult> GetItems(int wishlistId)
        {
            var response = await _wishlistItemService.GetItemsForWishlistAsync(wishlistId);
            return Ok(response);
        }

        [HttpGet("item/{itemId}")]
        public async Task<IActionResult> GetItem(int itemId)
        {
            var response = await _wishlistItemService.GetItemByIdAsync(itemId);
            return Ok(response);
        }
    }
}