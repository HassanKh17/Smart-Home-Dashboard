using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Views;

namespace SmartHomeDashboardP.ViewModels;
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Smart Home Dashboard";

    [RelayCommand]
    private async Task GoToSettings()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}
