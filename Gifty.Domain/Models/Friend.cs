namespace Gifty.Domain.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestStatus Status { get; set; } // Optional
        public string FriendName { get; set; }
        public DateTime FriendDOB { get; set; }
        public string FriendImage { get; set; }
    }

}