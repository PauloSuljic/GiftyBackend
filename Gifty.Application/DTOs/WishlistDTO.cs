namespace Gifty.Application.DTOs
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<WishlistItemDTO> Items { get; set; }
    }
}