using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Views;
using SmartHomeDashboardP.Models;
using SmartHomeDashboardP.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace SmartHomeDashboardP.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private readonly DeviceSimulationService _deviceService;

    [ObservableProperty]
    private string title = "Smart Home Dashboard";

    [ObservableProperty]
    private ObservableCollection<SmartDevice> devices;

    public MainViewModel(DeviceSimulationService deviceService)
    {
        _deviceService = deviceService;
        Devices = _deviceService.Devices;
    }

    [RelayCommand]
    private async Task GoToSettings()
    {
        await Shell.Current.GoToAsync(nameof(SettingsPage));
    }
}
