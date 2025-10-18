using SmartHomeDashboardP.ViewModels;

namespace SmartHomeDashboardP.Views;

public partial class DashboardPage : ContentPage
{
    public DashboardPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
