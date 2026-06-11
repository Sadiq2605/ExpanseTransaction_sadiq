using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services;

public class ReportService
{
    private readonly ExpenseDbContext _context;
    private readonly ILogger<ReportService> _logger;
    
    public ReportService(ExpenseDbContext context, ILogger<ReportService> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Dictionary<string, decimal>> GetExpensesByCategory()
    {
        var report = await _context.Expenses
            .GroupBy(e => e.Category)
            .Select(g => new { Category = g.Key, Total = g.Sum(e => e.Amount) })
            .ToDictionaryAsync(x => x.Category, x => x.Total);
        
        _logger.LogInformation("Generated expenses by category report");
        return report;
    }
    
    public async Task<decimal> GetMonthlyTotalAsync(int month, int year)
    {
        return await _context.Expenses
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .SumAsync(e => e.Amount);
    }
    
    public async Task<List<Expense>> GetExpensesInDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Expenses
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderByDescending(e => e.Date)
            .ToListAsync();
    }
    
    public async Task<decimal> GetAverageExpenseAsync()
    {
        var count = await _context.Expenses.CountAsync();
        
        if (count == 0)
            return 0;
        
        return await _context.Expenses.AverageAsync(e => e.Amount);
    }
    
    public async Task<Expense?> GetHighestExpenseAsync()
    {
        return await _context.Expenses
            .OrderByDescending(e => e.Amount)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Expense>> GetLowestExpensesAsync(int count = 5)
    {
        return await _context.Expenses
            .OrderBy(e => e.Amount)
            .Take(count)
            .ToListAsync();
    }
}
