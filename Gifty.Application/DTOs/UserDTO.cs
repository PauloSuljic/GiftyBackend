namespace Gifty.Application.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfilePictureURL { get; set; } // JWT token
    }
}