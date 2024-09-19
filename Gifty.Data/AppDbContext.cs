using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Gifty.Domain.Models;

namespace Gifty.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}