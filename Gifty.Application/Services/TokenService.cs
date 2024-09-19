using Gifty.Domain.Models;

namespace Gifty.Application.Services
{
    public class TokenService : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            // Your token creation logic here
            return "generatedToken";
        }
    }
}