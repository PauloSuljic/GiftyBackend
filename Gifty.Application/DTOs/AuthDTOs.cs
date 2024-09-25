namespace Gifty.Application.DTOs;

public class RegisterDTO
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public DateTime Birthday { get; set; }
}

public class LoginDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ResendConfirmationDTO
{
    public string Email { get; set; }
}

public class ForgotPasswordDTO
{
    public string Email { get; set; }
}

public class ResetPasswordDTO
{
    public string UserId { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}
