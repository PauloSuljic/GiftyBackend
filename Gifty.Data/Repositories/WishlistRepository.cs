using Gifty.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Data.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wishlist>> GetAllAsync()
        {
            return await _context.Wishlists.ToListAsync();
        }

        public async Task<Wishlist> GetByIdAsync(int id)
        {
            return await _context.Wishlists.FindAsync(id);
        }

        public async Task AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Wishlist wishlist)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
        }
    }
}