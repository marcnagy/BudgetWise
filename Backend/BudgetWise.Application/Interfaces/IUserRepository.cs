using BudgetWise.Domain.Entities;

namespace BudgetWise.Application.Interfaces;

/// <summary>
/// Interface for user repository.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByUsernameAsync(string Username);

}

