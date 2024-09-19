using Gifty.Data.Repositories;
using Gifty.Domain.Models;

namespace Gifty.Domain.Services;

public interface IUnitOfWork
{
    IRepository<Friend> Friends { get; }
    IRepository<Wishlist> Wishlists { get; }
    IRepository<BirthdayReminder> BirthdayReminders { get; }
    Task SaveChangesAsync();
}