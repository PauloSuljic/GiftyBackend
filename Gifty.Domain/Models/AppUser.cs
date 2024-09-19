using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Gifty.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}