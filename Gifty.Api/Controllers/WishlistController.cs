using System.Security.Claims;
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

        // Get wishlists for the logged-in user
        [HttpGet]
        public async Task<IActionResult> GetWishlists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            var wishlists = await _wishlistService.GetWishlistsByUserIdAsync(userId);
            return Ok(wishlists);
        }

        // Get a specific wishlist by ID
        [HttpGet("wishlist/{id:int}")]  // Explicitly say this route is for int IDs
        public async Task<IActionResult> GetWishlistById(int id)
        {
            var wishlist = await _wishlistService.GetWishlistByIdAsync(id);

            if (wishlist == null)
            {
                return NotFound($"Wishlist with ID {id} not found.");
            }

            return Ok(wishlist);
        }

        // Get all wishlists for a specific user by userId
        [HttpGet("user/{userId}")]  // Prefix the route for user-related actions
        public async Task<IActionResult> GetWishlistByUserId(string userId)
        {
            var wishlists = await _wishlistService.GetWishlistsByUserIdAsync(userId);
            return Ok(wishlists);
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