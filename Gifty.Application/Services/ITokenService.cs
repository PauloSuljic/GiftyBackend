using Gifty.Domain.Models;

namespace Gifty.Application.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}