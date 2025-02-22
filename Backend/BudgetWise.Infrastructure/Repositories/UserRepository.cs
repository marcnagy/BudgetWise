using BudgetWise.Application.Interfaces;
using BudgetWise.Domain.Entities;
using BudgetWise.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Repositories;

/// <summary>
/// Repository for users.
/// </summary>
public class UserRepository(AppDbContext context) : Repository<User>(context), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
{
    return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
}

    public async Task<User?> GetByUsernameAsync(string Username)
    {
    return await _dbSet.FirstOrDefaultAsync(u => u.Username == Username);
    }
}
