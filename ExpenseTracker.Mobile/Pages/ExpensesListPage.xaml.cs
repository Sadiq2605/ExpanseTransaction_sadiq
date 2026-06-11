namespace ExpenseTracker.Mobile.Pages;

public partial class ExpensesListPage : ContentPage
{
    private readonly ViewModels.ExpensesListViewModel _viewModel;

    public ExpensesListPage(ViewModels.ExpensesListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadExpensesCommand.ExecuteAsync(null);
    }
}
