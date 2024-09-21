using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Gifty.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Bio { get; set; }
        public string Location { get; set; }
        public string ProfilePictureUrl { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}