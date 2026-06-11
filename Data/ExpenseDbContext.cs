using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data;

public class ExpenseDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; } = null!;
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Using LocalDB - adjust connection string as needed
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ExpenseTrackerDb;Trusted_Connection=true;");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure Expense entity
        modelBuilder.Entity<Expense>()
            .HasKey(e => e.Id);
        
        modelBuilder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasPrecision(18, 2);
        
        // Add index for category searches
        modelBuilder.Entity<Expense>()
            .HasIndex(e => e.Category);
    }
}
