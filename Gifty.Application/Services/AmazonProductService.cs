using System.Text;
using Microsoft.Extensions.Configuration;

namespace Gifty.Application.Services;

public class AmazonProductService
{
    private readonly string _accessKeyId;
    private readonly string _secretAccessKey;
    private readonly string _associateTag;
    private readonly HttpClient _httpClient;

    public AmazonProductService(IConfiguration configuration)
    {
        _accessKeyId = configuration["AmazonSettings:AccessKeyId"];
        _secretAccessKey = configuration["AmazonSettings:SecretAccessKey"];
        _associateTag = configuration["AmazonSettings:AssociateTag"];
        _httpClient = new HttpClient();
    }

    private string SignRequest(string url)
    {
        // Generate the signature for the request
        var signature = ""; // Implement signing logic here based on Amazon's API requirements.
        return signature;
    }

    public async Task<string> GetProductDetailsAsync(string asin)
    {
        var endpoint = "webservices.amazon.com";
        var uri = "/onca/xml";
        var timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var parameters = new StringBuilder();
        parameters.Append($"Service=AWSECommerceService&Operation=ItemLookup&");
        parameters.Append($"AWSAccessKeyId={_accessKeyId}&");
        parameters.Append($"AssociateTag={_associateTag}&");
        parameters.Append($"ItemId={asin}&");
        parameters.Append($"ResponseGroup=Images,ItemAttributes,Offers&");
        parameters.Append($"Timestamp={timestamp}");

        // Generate the signature
        var signature = SignRequest(parameters.ToString());

        // Make the request
        var requestUrl = $"https://{endpoint}{uri}?{parameters}&Signature={signature}";

        var response = await _httpClient.GetAsync(requestUrl);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
