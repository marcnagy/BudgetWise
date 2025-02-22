namespace BudgetWise.Application.DTOs;

/// <summary>
/// Data transfer object for user registration.
/// </summary>
public class RegisterDTO
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}

/// <summary>
/// Data transfer object for user login.
/// </summary>
public class LoginDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

