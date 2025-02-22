using BudgetWise.Application.Interfaces;
using BudgetWise.Infrastructure.Repositories;

namespace BudgetWise.Infrastructure.Persistence;

/// <summary>
/// Unit of work implementation.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IExpenseRepository Expenses { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified application database context.
    /// </summary>
    /// <param name="context">The application database context to be used by the unit of work.</param>
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        Expenses = new ExpenseRepository(_context);
    }

    /// <summary>
    /// Saves all changes made in this unit of work to the database.
    /// </summary>
    /// <returns>The number of state entries written to the underlying database.</returns>
    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Releases all resources used by the unit of work.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
    }
}
