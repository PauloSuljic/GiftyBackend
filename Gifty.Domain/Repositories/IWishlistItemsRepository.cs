using Gifty.Data.Repositories;
using Gifty.Domain.Models;

namespace Gifty.Domain.Repositories
{
    public interface IWishlistItemRepository : IRepository<WishlistItem>
    {
        Task<IEnumerable<WishlistItem>> GetItemsByWishlistIdAsync(int wishlistId);
        Task<WishlistItem> GetItemByIdAsync(int itemId);
        Task RemoveByIdAsync(int itemId);
    }
}