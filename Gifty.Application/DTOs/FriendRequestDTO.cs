using Gifty.Domain.Models;

namespace Gifty.Application.DTOs;

public class FriendRequestDTO
{
    public int Id { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
