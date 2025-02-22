using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Domain.Entities;

/// <summary>
/// Represents an expense.
/// </summary>
public class Expense
{
    [Key]
    public int Id { get; set; }

    [Required]
    public float Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public required string Name { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public  User User { get; set; }
}
