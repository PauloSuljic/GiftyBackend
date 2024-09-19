namespace Gifty.Domain.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<WishlistItem> Items { get; set; }
    }
}