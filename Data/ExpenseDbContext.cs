using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ExpenseTracker.Models;

namespace ExpenseTracker.Data;

public class ExpenseDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    
    public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options, IConfiguration configuration) 
        : base(options)
    {
        _configuration = configuration;
    }
    
    public DbSet<Expense> Expenses { get; set; } = null!;
    
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
