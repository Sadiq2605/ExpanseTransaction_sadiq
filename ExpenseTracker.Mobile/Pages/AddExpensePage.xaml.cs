namespace ExpenseTracker.Mobile.Pages;

public partial class AddExpensePage : ContentPage
{
    private readonly ViewModels.AddExpenseViewModel _viewModel;

    public AddExpensePage(ViewModels.AddExpenseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}
