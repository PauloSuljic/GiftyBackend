namespace Gifty.Domain.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } // Optional
    }

}