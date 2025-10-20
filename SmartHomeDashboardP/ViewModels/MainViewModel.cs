using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Models;

namespace SmartHomeDashboardP.ViewModels;

public partial class MainViewModel : ObservableObject
{
    // Title for the dashboard
    [ObservableProperty]
    private string title = "Smart Home Dashboard";

    // ✅ Single collection of devices
    public ObservableCollection<SmartDevice> Devices { get; } = new();

    public MainViewModel()
    {
        // ✅ Initialize sample devices
        Devices.Add(new SmartDevice { Name = "Living Room Light", Status = "Off" });
        Devices.Add(new SmartDevice { Name = "Thermostat", Status = "22°C" });
        Devices.Add(new SmartDevice { Name = "Front Door Lock", Status = "Locked" });

        // ✅ Wire toggle commands
        foreach (var device in Devices)
        {
            device.ToggleCommand = new RelayCommand(() => ToggleDevice(device));
        }
    }

    // ✅ Toggle device states
    private void ToggleDevice(SmartDevice device)
    {
        if (device == null) return;

        if (device.Name.Contains("Light"))
            device.Status = device.Status == "On" ? "Off" : "On";

        else if (device.Name.Contains("Thermostat"))
            device.Status = device.Status == "22°C" ? "24°C" : "22°C";

        else if (device.Name.Contains("Lock"))
            device.Status = device.Status == "Locked" ? "Unlocked" : "Locked";
    }

    // ✅ Navigation to settings
    [RelayCommand]
    private async Task GoToSettings()
    {
        await Shell.Current.GoToAsync(nameof(Views.SettingsPage));
    }
}
