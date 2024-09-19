namespace Gifty.Application.DTOs
{
    public class EditWishlistDTO
    {
        public string Title { get; set; }
        public List<CreateWishlistItemDTO> Items { get; set; }
    }
}