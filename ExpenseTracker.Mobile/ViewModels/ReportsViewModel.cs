using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Shared.Services;
using System.Collections.ObjectModel;

namespace ExpenseTracker.Mobile.ViewModels
{
    public partial class ReportsViewModel : ObservableObject
    {
        private readonly ReportService _reportService;

        [ObservableProperty]
        ObservableCollection<CategoryReport> categoryReports = new();

        [ObservableProperty]
        decimal monthlyTotal;

        [ObservableProperty]
        decimal averageExpense;

        [ObservableProperty]
        string highestExpenseInfo = string.Empty;

        [ObservableProperty]
        bool isLoading;

        [ObservableProperty]
        DateTime selectedDate = DateTime.Now;

        public ReportsViewModel(ReportService reportService)
        {
            _reportService = reportService;
        }

        [RelayCommand]
        public async Task LoadReports()
        {
            try
            {
                IsLoading = true;

                var categoryReportData = await _reportService.GetExpensesByCategoryWithPercentageAsync();
                
                CategoryReports.Clear();
                foreach (var report in categoryReportData)
                {
                    CategoryReports.Add(new CategoryReport
                    {
                        Category = report.Category,
                        Total = report.Total,
                        Percentage = report.Percentage
                    });
                }

                MonthlyTotal = await _reportService.GetMonthlyTotalAsync(SelectedDate.Month, SelectedDate.Year);
                AverageExpense = await _reportService.GetAverageExpenseAsync();

                var highest = await _reportService.GetHighestExpenseAsync();
                HighestExpenseInfo = highest != null 
                    ? string.Format("{0} - ${1:F2}", highest.Description, highest.Amount)
                    : "N/A";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", string.Format("Failed to load reports: {0}", ex.Message), "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task DateChanged()
        {
            await LoadReportsCommand.ExecuteAsync(null);
        }
    }

    public class CategoryReport
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public double Percentage { get; set; }
        public string PercentageString => string.Format("{0:F1}%", Percentage);
    }
}
