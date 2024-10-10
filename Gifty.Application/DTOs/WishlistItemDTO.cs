namespace Gifty.Application.DTOs
{
    public class WishlistItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public decimal Price { get; set; }
        public int WishlistId { get; set; }
    }
}