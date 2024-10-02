namespace Gifty.Application.Services;

public interface IAmazonProductService
{
    Task<string> GetProductDetailsAsync(string asin);
}