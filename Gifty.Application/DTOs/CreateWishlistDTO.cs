namespace Gifty.Application.DTOs
{
    public class CreateWishlistDTO
    {
        public string Title { get; set; }
        public List<CreateWishlistItemDTO> Items { get; set; }
    }
}