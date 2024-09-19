namespace Gifty.Domain.Models;

public class FriendRequest
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public RequestStatus Status { get; set; } // Enum for Pending, Accepted, Declined
    public DateTime CreatedAt { get; set; }
}

public enum RequestStatus
{
    Pending,
    Accepted,
    Declined
}
