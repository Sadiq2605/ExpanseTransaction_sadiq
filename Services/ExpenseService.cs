using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services;

public class ExpenseService
{
    private readonly ExpenseDbContext _context;
    private readonly ILogger<ExpenseService> _logger;
    
    public ExpenseService(ExpenseDbContext context, ILogger<ExpenseService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task AddExpenseAsync(string description, decimal amount, string category)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.", nameof(category));
        
        var expense = new Expense
        {
            Description = description,
            Amount = amount,
            Category = category,
            Date = DateTime.Now
        };
        
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Expense added: {description} - ${amount} in {category}");
    }
    
    public async Task<List<Expense>> GetAllExpensesAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
    
    public async Task<List<Expense>> GetExpensesByCategoryAsync(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.", nameof(category));
        
        return await _context.Expenses
            .Where(e => e.Category.ToLower() == category.ToLower())
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
    
    public async Task<decimal> GetTotalExpensesAsync()
    {
        return await _context.Expenses.SumAsync(e => e.Amount);
    }
    
    public async Task<Expense?> GetExpenseByIdAsync(int id)
    {
        return await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id);
    }
    
    public async Task DeleteExpenseAsync(int id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {id} not found.");
        
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Expense deleted: ID {id}");
    }
    
    public async Task UpdateExpenseAsync(int id, string description, decimal amount, string category)
    {
        var expense = await _context.Expenses.FindAsync(id);
        
        if (expense == null)
            throw new InvalidOperationException($"Expense with ID {id} not found.");
        
        expense.Description = description ?? throw new ArgumentNullException(nameof(description));
        expense.Amount = amount > 0 ? amount : throw new ArgumentException("Amount must be greater than zero.");
        expense.Category = category ?? throw new ArgumentNullException(nameof(category));
        
        await _context.SaveChangesAsync();
        
        _logger.LogInformation($"Expense updated: ID {id}");
    }
}
