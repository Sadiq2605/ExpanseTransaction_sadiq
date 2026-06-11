namespace ExpenseTracker.Mobile.Pages;

public partial class EditExpensePage : ContentPage
{
    private readonly ViewModels.EditExpenseViewModel _viewModel;

    public EditExpensePage(ViewModels.EditExpenseViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }
}
