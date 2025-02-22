using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Application.DTOs;

/// <summary>
/// Data transfer object for expenses.
/// </summary>
public class ExpenseDTO
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public float Amount { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateTime Date { get; set; }

    public string? Description { get; set; }
}
