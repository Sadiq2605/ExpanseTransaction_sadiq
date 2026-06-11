namespace ExpenseTracker.Mobile.Pages;

public partial class ReportsPage : ContentPage
{
    private readonly ViewModels.ReportsViewModel _viewModel;

    public ReportsPage(ViewModels.ReportsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadReportsCommand.ExecuteAsync(null);
    }
}
