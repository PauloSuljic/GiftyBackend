using Gifty.Domain.Models;

namespace Gifty.Data.Repositories
{
    public class FriendRepository : Repository<Friend>, IFriendRepository
    {
        public FriendRepository(AppDbContext context) : base(context)
        {
        }

        // Add any specific implementations for Friend repository methods
    }
}