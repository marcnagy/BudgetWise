using BudgetWise.Application.Interfaces;
using BudgetWise.Domain.Entities;
using BudgetWise.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace BudgetWise.Infrastructure.Repositories;

/// <summary>
/// Repository for expenses.
/// </summary>
public class ExpenseRepository : Repository<Expense>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Expense>> GetExpensesByUserAsync(int userId)
    {
        return await _dbSet.Where(e => e.UserId == userId).ToListAsync();
    }
}
