namespace InforceTestTask.Application.Features.Users.Register;

public class RegisterUserDto
{
    public string NickName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string PasswordConfirm { get; set; } = string.Empty;
    
    public bool IsAdmin { get; set; }
}