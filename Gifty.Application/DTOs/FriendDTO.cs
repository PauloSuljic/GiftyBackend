using Gifty.Domain.Models;

namespace Gifty.Application.DTOs
{
    public class FriendDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime CreatedAt { get; set; }
        public RequestStatus Status { get; set; }
        public string FriendName { get; set; }
        public string FriendImage { get; set; }
        public DateTime FriendDOB { get; set; }
    }
}