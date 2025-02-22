namespace BudgetWise.Application.Interfaces;

/// <summary>
/// Interface for unit of work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IExpenseRepository Expenses { get; }
    Task<int> CompleteAsync();
}
