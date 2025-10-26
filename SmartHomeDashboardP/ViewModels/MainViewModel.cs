using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Timers;
using SmartHomeDashboardP.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Services;

namespace SmartHomeDashboardP.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "Smart Home Dashboard";

    // ✅ Nullable chart prevents null-reference during startup
    [ObservableProperty]
    private Chart? energyUsageChart;

    public ObservableCollection<SmartDevice> Devices { get; } = new();


    private readonly List<EnergyRecord> _energyHistory = new();
    public IReadOnlyList<EnergyRecord> EnergyHistory => _energyHistory.AsReadOnly();
    
    private readonly EnergyHistoryService _historyService = new();

    private readonly System.Timers.Timer _updateTimer;
    private readonly Random _rand = new();

    public MainViewModel()
    {
        try
        {
            _energyHistory = _historyService.LoadHistory();
            // ✅ Initialize devices
            Devices.Add(new SmartDevice { Name = "Living Room Light", Status = "Off", Icon = "💡" , Category="Lighting", PowerUsage=40});
            Devices.Add(new SmartDevice { Name = "Thermostat", Status = "22°C", Icon = "🌡️", Category="Climate", PowerUsage=60 });
            Devices.Add(new SmartDevice { Name = "Front Door Lock", Status = "Locked", Icon = "🔒", Category="Security", PowerUsage=5 });

            // ✅ Wire toggle commands
            foreach (var device in Devices)
                device.ToggleCommand = new RelayCommand(() => ToggleDevice(device));

            // ✅ Initialize chart once UI is ready
            MainThread.BeginInvokeOnMainThread(UpdateEnergyUsageChart);

            // ✅ Setup safe background timer
            _updateTimer = new System.Timers.Timer(3000)
            {
                AutoReset = true,
                Enabled = false // start later to avoid race condition
            };

            _updateTimer.Elapsed += (_, _) =>
            {
                try
                {
                    // Always update chart on main thread
                    MainThread.BeginInvokeOnMainThread(UpdateEnergyUsageChart);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Timer Error] {ex.Message}");
                }
            };

            // 🕒 Delay start: ensures page + bindings are ready before timer fires
            Application.Current?.Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(3), () =>
            {
                try
                {
                    _updateTimer.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Timer Start Error] {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Init Error] {ex.Message}");
        }
    }

    // ✅ Toggle Device State
    private void ToggleDevice(SmartDevice device)
    {
        try
        {
            if (device.Name.Contains("Light"))
                device.Status = device.Status == "On" ? "Off" : "On";
            else if (device.Name.Contains("Thermostat"))
                device.Status = device.Status == "22°C" ? "24°C" : "22°C";
            else if (device.Name.Contains("Lock"))
                device.Status = device.Status == "Locked" ? "Unlocked" : "Locked";

            // Refresh chart immediately
            MainThread.BeginInvokeOnMainThread(UpdateEnergyUsageChart);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Toggle Error] {ex.Message}");
        }
    }

    // ✅ Chart Update Logic (Main Thread Safe)
    private void UpdateEnergyUsageChart()
    {
        try
        {
            if (Devices == null || Devices.Count == 0)
                return;

            int baseLoad = 100;
            int lights = Devices.Count(d => d.Name.Contains("Light") && d.Status == "On") * 40;
            int thermostat = Devices.Count(d => d.Name.Contains("Thermostat")) * 20;
            int locks = Devices.Count(d => d.Name.Contains("Lock") && d.Status == "Unlocked") * 5;
            int total = baseLoad + lights + thermostat + locks + _rand.Next(0, 30);

            _energyHistory.Add(new EnergyRecord
            {
                Timestamp = DateTime.Now,
                TotalWatts = total
            });

            // ✅ Keep list size manageable (e.g., last 50 data points)
            if (_energyHistory.Count > 50)
                _energyHistory.RemoveAt(0);
            _historyService.SaveHistory(_energyHistory);

            var entries = new[]
            {
                new ChartEntry(baseLoad){ Label="Base", ValueLabel=$"{baseLoad}W", Color=SKColor.Parse("#90CAF9") },
                new ChartEntry(lights){ Label="Lights", ValueLabel=$"{lights}W", Color=SKColor.Parse("#FFD54F") },
                new ChartEntry(thermostat){ Label="Thermostat", ValueLabel=$"{thermostat}W", Color=SKColor.Parse("#FF8A65") },
                new ChartEntry(locks){ Label="Locks", ValueLabel=$"{locks}W", Color=SKColor.Parse("#A5D6A7") },
                new ChartEntry(total){ Label="Total", ValueLabel=$"{total}W", Color=SKColor.Parse("#42A5F5") }
            };

            EnergyUsageChart = new DonutChart
            {
                Entries = entries,
                BackgroundColor = SKColor.Parse("#FFFFFF"),
                HoleRadius = 0.5f,
                LabelTextSize = 32
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Chart Update Error] {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GoToSettings()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(Views.SettingsPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Navigation Error] {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task GoToHistory()
    {
        try
        {
            await Shell.Current.GoToAsync(nameof(Views.EnergyHistoryPage));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Navigation Error] {ex.Message}");
        }
    }

}
