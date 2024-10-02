using Microsoft.AspNetCore.Mvc;
using Gifty.Application.DTOs;
using Gifty.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace Gifty.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlists()
        {
            var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var wishlists = await _wishlistService.GetWishlistsByUserIdAsync(userId);
            return Ok(wishlists);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlistById(int id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);

            if (wishlist == null)
            {
                return NotFound($"Wishlist with ID {id} not found.");
            }

            return Ok(wishlist);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDTO wishlistDto)
        {
            var result = await _wishlistService.CreateWishlistAsync(wishlistDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWishlist(int id, EditWishlistDTO wishlistDto)
        {
            var result = await _wishlistService.UpdateWishlistAsync(id, wishlistDto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            var result = await _wishlistService.DeleteWishlistAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}