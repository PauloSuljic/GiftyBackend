// Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using Gifty.Application.Services;

namespace Gifty.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AmazonProductService _amazonProductService;

    public ProductController(AmazonProductService amazonProductService)
    {
        _amazonProductService = amazonProductService;
    }

    [HttpPost("details")]
    public async Task<IActionResult> GetProductDetails([FromBody] string asin)
    {
        try
        {
            var productDetails = await _amazonProductService.GetProductDetailsAsync(asin);
            return Ok(productDetails);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error fetching product details: {ex.Message}");
        }
    }
}