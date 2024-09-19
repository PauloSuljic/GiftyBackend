namespace Gifty.Application.DTOs
{
    public class UserDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } // JWT token
    }
}