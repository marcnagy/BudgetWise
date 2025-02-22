using BudgetWise.Application.Interfaces;
using BudgetWise.Domain.Entities;

namespace BudgetWise.Application.Services;

/// <summary>
/// Service for managing expenses.
/// </summary>
public class ExpenseService(IExpenseRepository expenseRepository) : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository = expenseRepository;

    /// <summary>
    /// Get all expenses for the user with the given ID.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve expenses for.</param>
    /// <returns>An asynchronous sequence of <see cref="Expense"/> for the given user ID.</returns>
    public async Task<IEnumerable<Expense>> GetAllExpensesAsync(int userId)
    {
        return await _expenseRepository.GetExpensesByUserAsync(userId);
    }

    /// <summary>
    /// Get the expense with the given ID.
    /// </summary>
    /// <param name="id">The ID of the expense to retrieve.</param>
    /// <returns>An asynchronous sequence of <see cref="Expense"/> for the given ID.</returns>
    public async Task<Expense> GetExpenseByIdAsync(int id)
    {
        return await _expenseRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Add the given expense to the database.
    /// </summary>
    /// <param name="expense">The expense to add.</param>
    /// <returns>An asynchronous task indicating the completion of the operation.</returns>
    public async Task AddExpenseAsync(Expense expense)
    {
         await _expenseRepository.AddAsync(expense);
    }

    /// <summary>
    /// Update an expense in the database.
    /// </summary>
    /// <param name="expense">The expense to update.</param>
    /// <returns>An asynchronous task indicating the completion of the operation.</returns>
    public async Task UpdateExpenseAsync(Expense expense)
    {
         await _expenseRepository.UpdateAsync(expense);
    }

    /// <summary>
    /// Delete an expense from the database.
    /// </summary>
    /// <param name="id">The ID of the expense to delete.</param>
    /// <returns>An asynchronous task indicating the success of the operation.</returns>
    public async Task<bool> DeleteExpenseAsync(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);
        if (expense == null) return false;

        await _expenseRepository.RemoveAsync(expense);
        return true;
    }
}
