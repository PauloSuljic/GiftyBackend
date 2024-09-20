namespace Gifty.Application.DTOs
{
    public class FriendDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}