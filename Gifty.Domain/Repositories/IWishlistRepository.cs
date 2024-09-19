using Gifty.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gifty.Data.Repositories
{
    public interface IWishlistRepository
    {
        Task<IEnumerable<Wishlist>> GetAllAsync();
        Task<Wishlist> GetByIdAsync(int id);
        Task AddAsync(Wishlist wishlist);
        Task UpdateAsync(Wishlist wishlist);
        Task DeleteAsync(Wishlist wishlist);
    }
}