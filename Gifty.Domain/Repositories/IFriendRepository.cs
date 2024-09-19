using Gifty.Domain.Models;

namespace Gifty.Domain.Repositories
{
    public interface IFriendRepository
    {
        Task<IEnumerable<Friend>> GetFriendsByUserIdAsync(string userId);
        Task<Friend?> GetFriendByIdAndUserIdAsync(int friendId, string userId); // Nullable return
        Task AddAsync(Friend friend);
        Task DeleteAsync(Friend friend);
    }
}