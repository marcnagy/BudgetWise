﻿using System.Linq.Expressions;

namespace BudgetWise.Application.Interfaces;

/// <summary>
/// Interface for repository.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveChangesAsync();
}

