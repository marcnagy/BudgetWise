using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BudgetWise.Application.DTOs;
using BudgetWise.Application.Interfaces;
using BudgetWise.Domain.Entities;
using BudgetWise.API.Constants;

namespace BudgetWise.API.Controllers;

/// <summary>
/// Controller for user authentication and registration.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController(IUserRepository userRepository, IConfiguration configuration, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<AuthController> _logger = logger;

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="dto">The user registration data.</param>
    /// <returns>A 200 OK response with a success message, or a 400 Bad Request response with errors if the model is invalid, or a 409 Conflict response with an error message if the email is already registered, or a 500 Internal Server Error response with an error message if an exception occurs.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        // Validate model
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = Messages.InvalidRequest, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        // Check if email is already registered
        var existingUserByEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUserByEmail != null)
        {
            _logger.LogError(Messages.EmailAlreadyExists);
            return Conflict(new { message = Messages.EmailAlreadyExists });
        }

        // Check if username is already taken
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existingUserByUsername != null)
        {
            _logger.LogError(Messages.UsernameAlreadyExists);
            return Conflict(new { message = Messages.UsernameAlreadyExists });
        }

        // Validate password strength
        if (!IsPasswordStrong(dto.Password))
        {
            _logger.LogError(Messages.PasswordTooWeak);
            return BadRequest(new { message = Messages.PasswordTooWeak });
        }

        // Create new user
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        try
        {
            await _userRepository.AddAsync(user);
            _logger.LogError(Messages.UserRegistered);
            return Ok(new { message = Messages.UserRegistered });
        }
        catch (Exception ex)
        {
            _logger.LogError(Messages.InternalServerError);
            return StatusCode(500, new { message = Messages.InternalServerError, error = ex.Message });
        }
    }

    /// <summary>
    /// Logs in a user with their email and password, and returns a JSON Web Token to be used in subsequent requests.
    /// </summary>
    /// <param name="dto">The login data.</param>
    /// <returns>A JSON Web Token.</returns>
    /// <response code="200">The user was logged in successfully.</response>
    /// <response code="400">The request was invalid.</response>
    /// <response code="401">The email or password was incorrect.</response>
    /// <response code="500">An internal server error occurred.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        // Validate model
        if (!ModelState.IsValid)
        {
            return BadRequest(new { message = Messages.InvalidRequest, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
        }

        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogError(Messages.InvalidEmailOrPassword);
            return Unauthorized(new { message = Messages.InvalidEmailOrPassword });
        }

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    /// <summary>
    /// Checks if the specified password is strong.
    /// A strong password is at least 8 characters long and contains at least one uppercase letter, one lowercase letter and one digit.
    /// </summary>
    /// <param name="password">The password to check.</param>
    /// <returns>true if the password is strong, otherwise false.</returns>
    private static bool IsPasswordStrong(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit);
    }

    /// <summary>
    /// Generates a JSON Web Token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A signed JSON Web Token as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the JWT secret key is not configured properly.</exception>
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var secretKey = _configuration["JwtSettings:Secret"];

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured properly.");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
