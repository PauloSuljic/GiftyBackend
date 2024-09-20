using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gifty.Data.Repositories
{
    public class WishlistItemRepository : Repository<WishlistItem>, IWishlistItemRepository
    {
        private readonly AppDbContext _context;

        public WishlistItemRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistItem>> GetItemsByWishlistIdAsync(int wishlistId)
        {
            return await _context.WishlistItems
                .Where(item => item.WishlistId == wishlistId)
                .ToListAsync();
        }

        public async Task<WishlistItem> GetItemByIdAsync(int itemId)
        {
            return await _context.WishlistItems
                .FirstOrDefaultAsync(item => item.Id == itemId);
        }
        
        public async Task RemoveByIdAsync(int itemId)
        {
            var item = await GetItemByIdAsync(itemId);
            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}