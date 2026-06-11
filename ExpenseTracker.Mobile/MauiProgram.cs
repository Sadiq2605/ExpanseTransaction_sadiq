using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Mobile.Pages;
using ExpenseTracker.Mobile.ViewModels;
using ExpenseTracker.Shared.Data;
using ExpenseTracker.Shared.Services;

namespace ExpenseTracker.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
            });

        // Configure Database
        var dbPath = ExpenseDbContext.GetDatabasePath();
        builder.Services.AddDbContext<ExpenseDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}")
        );

        // Register Services
        builder.Services.AddScoped<ExpenseService>();
        builder.Services.AddScoped<ReportService>();

        // Register ViewModels
        builder.Services.AddSingleton<DashboardViewModel>();
        builder.Services.AddSingleton<AddExpenseViewModel>();
        builder.Services.AddSingleton<ExpensesListViewModel>();
        builder.Services.AddSingleton<ReportsViewModel>();
        builder.Services.AddSingleton<EditExpenseViewModel>();

        // Register Pages
        builder.Services.AddSingleton<DashboardPage>();
        builder.Services.AddSingleton<AddExpensePage>();
        builder.Services.AddSingleton<ExpensesListPage>();
        builder.Services.AddSingleton<ReportsPage>();
        builder.Services.AddSingleton<EditExpensePage>();
        builder.Services.AddSingleton<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
