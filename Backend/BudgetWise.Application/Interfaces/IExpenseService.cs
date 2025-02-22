using BudgetWise.Domain.Entities;

namespace BudgetWise.Application.Interfaces;

/// <summary>
/// Interface for expense service.
/// </summary>
public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetAllExpensesAsync(int userId);
    Task<Expense> GetExpenseByIdAsync(int id);
    Task AddExpenseAsync(Expense expense);
    Task UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int id);
}
