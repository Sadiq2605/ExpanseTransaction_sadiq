namespace ExpenseTracker.Mobile.Pages;

public partial class DashboardPage : ContentPage
{
    private readonly ViewModels.DashboardViewModel _viewModel;

    public DashboardPage(ViewModels.DashboardViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadDashboardCommand.ExecuteAsync(null);
    }
}
