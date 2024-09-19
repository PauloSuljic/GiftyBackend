namespace Gifty.Domain.Models;

public class WishlistItem
{
    public int Id { get; set; }
    public string Link { get; set; }
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; }
}