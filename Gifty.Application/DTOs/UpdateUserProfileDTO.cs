namespace Gifty.Application.DTOs;

public class UpdateUserProfileDTO
{
    public string FullName { get; set; }
    public DateTime? Birthday { get; set; }
    public string Bio { get; set; }
    public string Location { get; set; }
    public string ProfilePictureUrl { get; set; }
}
