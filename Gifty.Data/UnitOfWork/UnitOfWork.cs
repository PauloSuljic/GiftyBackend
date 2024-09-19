using Gifty.Data.Repositories;
using Gifty.Domain.Models;
using Gifty.Domain.Services;

namespace Gifty.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    public IRepository<Friend> Friends { get; }
    public IRepository<Wishlist> Wishlists { get; }
    public IRepository<BirthdayReminder> BirthdayReminders { get; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Friends = new Repository<Friend>(context);
        Wishlists = new Repository<Wishlist>(context);
        BirthdayReminders = new Repository<BirthdayReminder>(context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}