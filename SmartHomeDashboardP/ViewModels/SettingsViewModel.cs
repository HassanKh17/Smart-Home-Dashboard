using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Models;
using SmartHomeDashboardP.Services;

namespace SmartHomeDashboardP.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly SettingsService _settingsService = new();

    [ObservableProperty]
    private int updateIntervalSeconds;

    [ObservableProperty]
    private bool isDarkTheme;

    [ObservableProperty]
    private bool enableSimulation;

    public SettingsViewModel()
    {
        var settings = _settingsService.LoadSettings();
        UpdateIntervalSeconds = settings.UpdateIntervalSeconds;
        IsDarkTheme = settings.IsDarkTheme;
        EnableSimulation = settings.EnableSimulation;
    }

    [RelayCommand]
    private void Save()
    {
        var newSettings = new AppSettings
        {
            UpdateIntervalSeconds = UpdateIntervalSeconds,
            IsDarkTheme = IsDarkTheme,
            EnableSimulation = EnableSimulation
        };

        _settingsService.SaveSettings(newSettings);

        Application.Current?.MainPage?.DisplayAlert("Settings Saved", "Changes applied successfully!", "OK");

        // Apply theme immediately
        App.Current!.UserAppTheme = IsDarkTheme ? AppTheme.Dark : AppTheme.Light;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
