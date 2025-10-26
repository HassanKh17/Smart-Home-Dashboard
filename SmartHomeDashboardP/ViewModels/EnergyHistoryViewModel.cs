using Microcharts;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Models;

namespace SmartHomeDashboardP.ViewModels;

public partial class EnergyHistoryViewModel : ObservableObject
{
    [ObservableProperty]
    private Chart? _energyHistoryChart;
    public Chart? energyHistoryChart
    {
        get => _energyHistoryChart;
        set => SetProperty(ref _energyHistoryChart, value);
    }

    private readonly IEnumerable<EnergyRecord> _records;

    public EnergyHistoryViewModel()
    {
        // ✅ Retrieve history from the MainViewModel (if active)
        if (Application.Current?.MainPage?.BindingContext is MainViewModel mainVm)
        {
            _records = mainVm.EnergyHistory;
        }
        else
        {
            _records = Enumerable.Empty<EnergyRecord>();
        }

        LoadChart();
    }

    private void LoadChart()
    {
        if (!_records.Any())
        {
            EnergyHistoryChart = new LineChart
            {
                Entries = new[]
                {
                    new ChartEntry(0)
                    {
                        Label = "No data yet",
                        ValueLabel = "0W",
                        Color = SKColor.Parse("#FF5252")
                    }
                },
                LabelTextSize = 28,
                BackgroundColor = SKColor.Parse("#FFFFFF")
            };
            return;
        }

        var entries = _records.Select(r =>
            new ChartEntry(r.TotalWatts)
            {
                Label = r.Timestamp.ToString("HH:mm:ss"),
                ValueLabel = $"{r.TotalWatts}W",
                Color = SKColor.Parse("#42A5F5")
            }).ToArray();

        EnergyHistoryChart = new LineChart
        {
            Entries = entries,
            LineMode = LineMode.Straight,
            LineSize = 6,
            LabelTextSize = 28,
            PointMode = PointMode.Circle,
            PointSize = 8,
            BackgroundColor = SKColor.Parse("#FFFFFF")
        };
    }

    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}
