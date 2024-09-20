using System.ComponentModel.DataAnnotations.Schema;

namespace Gifty.Domain.Models;

public class WishlistItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; }
}