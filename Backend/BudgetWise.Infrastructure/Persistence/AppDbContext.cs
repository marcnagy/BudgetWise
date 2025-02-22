using BudgetWise.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Persistence;

/// <summary>
/// Application database context.
/// </summary>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    /// <summary>
    /// Configures the database model.
    /// </summary>
    /// <remarks>
    /// Configures the following relationships:
    /// <list type="bullet">
    /// <item><description>User -> Expenses (One-to-Many)</description></item>
    /// </list>
    /// </remarks>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> being configured.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User -> Expenses (One-to-Many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Expenses)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
