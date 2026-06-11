using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Services;
using ExpenseTracker.Data;

namespace ExpenseTracker;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        // Setup Dependency Injection
        var services = new ServiceCollection();
        
        // Add configuration
        services.AddSingleton<IConfiguration>(config);
        
        // Add logging
        services.AddLogging(logConfig =>
        {
            logConfig.AddConsole();
            logConfig.SetMinimumLevel(LogLevel.Information);
        });
        
        // Add DbContext with PostgreSQL (Supabase)
        var connectionString = config.GetConnectionString("DefaultConnection");
        services.AddDbContext<ExpenseDbContext>(options =>
            options.UseNpgsql(connectionString)
        );
        
        services.AddScoped<ExpenseService>();
        services.AddScoped<ReportService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        // Get the main application service
        var expenseService = serviceProvider.GetRequiredService<ExpenseService>();
        var reportService = serviceProvider.GetRequiredService<ReportService>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("Expense Tracker Application Started");
        logger.LogInformation($"Connected to Supabase Database");
        
        try
        {
            await RunApplication(expenseService, reportService, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred");
        }
        finally
        {
            await serviceProvider.DisposeAsync();
        }
    }
    
    static async Task RunApplication(ExpenseService expenseService, ReportService reportService, ILogger<Program> logger)
    {
        bool running = true;
        
        while (running)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║     EXPENSE TRACKER APPLICATION        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\n📊 MAIN MENU:");
            Console.WriteLine("1. ➕ Add Expense");
            Console.WriteLine("2. 📋 View All Expenses");
            Console.WriteLine("3. 🏷️  View Expenses by Category");
            Console.WriteLine("4. 💰 Get Total Expenses");
            Console.WriteLine("5. 📈 View Reports");
            Console.WriteLine("6. ❌ Delete Expense");
            Console.WriteLine("7. 🚪 Exit");
            Console.Write("\nSelect an option (1-7): ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    await AddExpense(expenseService);
                    break;
                case "2":
                    await ViewAllExpenses(expenseService);
                    break;
                case "3":
                    await ViewExpensesByCategory(expenseService);
                    break;
                case "4":
                    await GetTotalExpenses(expenseService);
                    break;
                case "5":
                    await ViewReports(reportService);
                    break;
                case "6":
                    await DeleteExpense(expenseService);
                    break;
                case "7":
                    running = false;
                    logger.LogInformation("Application terminated");
                    Console.WriteLine("\n👋 Thank you for using Expense Tracker!");
                    break;
                default:
                    Console.WriteLine("\n❌ Invalid option. Please try again.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    static async Task AddExpense(ExpenseService expenseService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║         ADD NEW EXPENSE                ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            Console.Write("📝 Enter description: ");
            string description = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("❌ Description cannot be empty!");
                return;
            }
            
            Console.Write("💵 Enter amount: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                Console.WriteLine("❌ Invalid amount. Please enter a valid number.");
                return;
            }
            
            if (amount <= 0)
            {
                Console.WriteLine("❌ Amount must be greater than zero.");
                return;
            }
            
            Console.Write("🏷️  Enter category (e.g., Food, Transport, Entertainment, Utilities): ");
            string category = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("❌ Category cannot be empty!");
                return;
            }
            
            await expenseService.AddExpenseAsync(description, amount, category);
            Console.WriteLine($"\n✅ Expense added successfully!");
            Console.WriteLine($"   Description: {description}");
            Console.WriteLine($"   Amount: ${amount:F2}");
            Console.WriteLine($"   Category: {category}");
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ViewAllExpenses(ExpenseService expenseService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       ALL EXPENSES                     ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            var expenses = await expenseService.GetAllExpensesAsync();
            
            if (!expenses.Any())
            {
                Console.WriteLine("📭 No expenses found.");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"{"ID",-5} {"Description",-20} {"Amount",-12} {"Category",-15} {"Date",-12}");
            Console.WriteLine(new string('─', 65));
            
            foreach (var expense in expenses)
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Description,-20} ${expense.Amount,-11:F2} {expense.Category,-15} {expense.Date:yyyy-MM-dd}");
            }
            
            decimal total = expenses.Sum(e => e.Amount);
            Console.WriteLine(new string('─', 65));
            Console.WriteLine($"{'Total:',-38} ${total:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ViewExpensesByCategory(ExpenseService expenseService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║    EXPENSES BY CATEGORY                ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            Console.Write("🏷️  Enter category name: ");
            string category = Console.ReadLine() ?? "";
            
            if (string.IsNullOrWhiteSpace(category))
            {
                Console.WriteLine("❌ Category cannot be empty!");
                return;
            }
            
            var expenses = await expenseService.GetExpensesByCategoryAsync(category);
            
            if (!expenses.Any())
            {
                Console.WriteLine($"\n📭 No expenses found in '{category}' category.");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"\n📊 Expenses in '{category}' category:\n");
            Console.WriteLine($"{"ID",-5} {"Description",-20} {"Amount",-12} {"Date",-12}");
            Console.WriteLine(new string('─', 50));
            
            foreach (var expense in expenses)
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Description,-20} ${expense.Amount,-11:F2} {expense.Date:yyyy-MM-dd}");
            }
            
            decimal total = expenses.Sum(e => e.Amount);
            decimal average = expenses.Count > 0 ? total / expenses.Count : 0;
            
            Console.WriteLine(new string('─', 50));
            Console.WriteLine($"Total in {category}: ${total:F2}");
            Console.WriteLine($"Average: ${average:F2}");
            Console.WriteLine($"Count: {expenses.Count}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task GetTotalExpenses(ExpenseService expenseService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║       TOTAL EXPENSES                   ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            decimal total = await expenseService.GetTotalExpensesAsync();
            Console.WriteLine($"💰 Total Expenses: ${total:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ViewReports(ReportService reportService)
    {
        bool reportMenu = true;
        
        while (reportMenu)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║           REPORTS MENU                 ║");
            Console.WriteLine("╚════════════════════════════════════════╝");
            Console.WriteLine("\n1. 📊 Expenses by Category");
            Console.WriteLine("2. 📅 Monthly Expenses");
            Console.WriteLine("3. 📈 Average Expense");
            Console.WriteLine("4. 🔝 Highest Expense");
            Console.WriteLine("5. 📆 Expenses in Date Range");
            Console.WriteLine("6. ⬅️  Back to Main Menu");
            Console.Write("\nSelect an option (1-6): ");
            
            string choice = Console.ReadLine() ?? "";
            
            switch (choice)
            {
                case "1":
                    await ShowExpensesByCategory(reportService);
                    break;
                case "2":
                    await ShowMonthlyExpenses(reportService);
                    break;
                case "3":
                    await ShowAverageExpense(reportService);
                    break;
                case "4":
                    await ShowHighestExpense(reportService);
                    break;
                case "5":
                    await ShowExpensesByDateRange(reportService);
                    break;
                case "6":
                    reportMenu = false;
                    break;
                default:
                    Console.WriteLine("❌ Invalid option.");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
    
    static async Task ShowExpensesByCategory(ReportService reportService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   EXPENSES BY CATEGORY REPORT          ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            var report = await reportService.GetExpensesByCategory();
            
            if (!report.Any())
            {
                Console.WriteLine("📭 No expenses found.");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"{"Category",-20} {"Total Amount",-15} {"Percentage",-15}");
            Console.WriteLine(new string('─', 50));
            
            decimal grandTotal = report.Values.Sum();
            
            foreach (var kvp in report.OrderByDescending(x => x.Value))
            {
                decimal percentage = (kvp.Value / grandTotal) * 100;
                Console.WriteLine($"{kvp.Key,-20} ${kvp.Value,-14:F2} {percentage,6:F1}%");
            }
            
            Console.WriteLine(new string('─', 50));
            Console.WriteLine($"{"TOTAL",-20} ${grandTotal:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ShowMonthlyExpenses(ReportService reportService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║      MONTHLY EXPENSES REPORT           ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            Console.Write("📅 Enter month (1-12): ");
            if (!int.TryParse(Console.ReadLine(), out int month) || month < 1 || month > 12)
            {
                Console.WriteLine("❌ Invalid month.");
                return;
            }
            
            Console.Write("📅 Enter year (e.g., 2024): ");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("❌ Invalid year.");
                return;
            }
            
            decimal total = await reportService.GetMonthlyTotalAsync(month, year);
            
            Console.WriteLine($"\n💰 Total expenses for {month:00}/{year}: ${total:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ShowAverageExpense(ReportService reportService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║     AVERAGE EXPENSE REPORT             ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            decimal average = await reportService.GetAverageExpenseAsync();
            
            Console.WriteLine($"📊 Average Expense: ${average:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ShowHighestExpense(ReportService reportService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║      HIGHEST EXPENSE REPORT            ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            var expense = await reportService.GetHighestExpenseAsync();
            
            if (expense == null)
            {
                Console.WriteLine("📭 No expenses found.");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"🔝 Highest Expense Details:");
            Console.WriteLine($"   Description: {expense.Description}");
            Console.WriteLine($"   Amount: ${expense.Amount:F2}");
            Console.WriteLine($"   Category: {expense.Category}");
            Console.WriteLine($"   Date: {expense.Date:yyyy-MM-dd}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task ShowExpensesByDateRange(ReportService reportService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║   EXPENSES BY DATE RANGE REPORT        ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            Console.Write("📅 Enter start date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
            {
                Console.WriteLine("❌ Invalid date format.");
                return;
            }
            
            Console.Write("📅 Enter end date (yyyy-MM-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
            {
                Console.WriteLine("❌ Invalid date format.");
                return;
            }
            
            if (startDate > endDate)
            {
                Console.WriteLine("❌ Start date cannot be after end date.");
                return;
            }
            
            var expenses = await reportService.GetExpensesInDateRangeAsync(startDate, endDate);
            
            if (!expenses.Any())
            {
                Console.WriteLine($"\n📭 No expenses found between {startDate:yyyy-MM-dd} and {endDate:yyyy-MM-dd}.");
                Console.Write("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine($"\n📊 Expenses from {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}:\n");
            Console.WriteLine($"{"ID",-5} {"Description",-20} {"Amount",-12} {"Category",-15} {"Date",-12}");
            Console.WriteLine(new string('─', 65));
            
            foreach (var expense in expenses)
            {
                Console.WriteLine($"{expense.Id,-5} {expense.Description,-20} ${expense.Amount,-11:F2} {expense.Category,-15} {expense.Date:yyyy-MM-dd}");
            }
            
            decimal total = expenses.Sum(e => e.Amount);
            Console.WriteLine(new string('─', 65));
            Console.WriteLine($"{'Total:',-38} ${total:F2}");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
    
    static async Task DeleteExpense(ExpenseService expenseService)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════╗");
            Console.WriteLine("║        DELETE EXPENSE                  ║");
            Console.WriteLine("╚════════════════════════════════════════╝\n");
            
            Console.Write("🔍 Enter expense ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("❌ Invalid ID.");
                return;
            }
            
            await expenseService.DeleteExpenseAsync(id);
            Console.WriteLine($"\n✅ Expense (ID: {id}) deleted successfully!");
            
            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}");
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
