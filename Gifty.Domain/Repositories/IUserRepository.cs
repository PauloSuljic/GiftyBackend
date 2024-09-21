using Gifty.Data.Repositories;
using Gifty.Domain.Models;

namespace Gifty.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(string userId); // To get a user by ID
        Task<IEnumerable<AppUser>> GetAllAsync(); // To get all users
        Task UpdateAsync(AppUser user);
    }
}