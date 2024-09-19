using Microsoft.AspNetCore.Mvc;
using Gifty.Application.DTOs;
using Gifty.Application.Services;

namespace Gifty.API.Controllers
{
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
            var wishlists = await _wishlistService.GetWishlistsAsync();
            return Ok(wishlists);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWishlist(CreateWishlistDTO wishlistDto)
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