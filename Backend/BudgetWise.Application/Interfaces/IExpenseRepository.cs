using BudgetWise.Domain.Entities;

namespace BudgetWise.Application.Interfaces;

/// <summary>
/// Interface for expense repository.
/// </summary>
public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId);
}