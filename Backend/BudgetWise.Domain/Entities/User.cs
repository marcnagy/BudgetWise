using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Domain.Entities;

/// <summary>
/// Represents a user.
/// </summary>
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public required string Username { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
