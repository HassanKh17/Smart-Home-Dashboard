using Microcharts;
using SkiaSharp;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartHomeDashboardP.Models;
using SmartHomeDashboardP.Services;

namespace SmartHomeDashboardP.ViewModels;

public partial class EnergyHistoryViewModel : ObservableObject
{
    [ObservableProperty]
    private Chart? energyHistoryChart;

    private readonly EnergyHistoryService _historyService = new();

    public EnergyHistoryViewModel()
    {
        LoadChart();
    }

    private void LoadChart()
    {
        var records = _historyService.LoadHistory();

        if (records == null || records.Count == 0)
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
                LineMode = LineMode.Straight,
                LineSize = 4,
                LabelTextSize = 28,
                BackgroundColor = SKColor.Parse("#F5F5F5"),
                LabelColor = SKColor.Parse("#555555"),
            };
            return;
        }

        // 🧠 Simplify timestamps (e.g., show every Nth to avoid clutter)
        var reducedRecords = records
            .Where((r, i) => i % Math.Max(1, records.Count / 8) == 0) // show ~8 labels
            .ToList();

        var entries = records.Select(r =>
            new ChartEntry(r.TotalWatts)
            {
                Label = reducedRecords.Contains(r)
                    ? r.Timestamp.ToString("HH:mm:ss")
                    : string.Empty, // only label some points
                ValueLabel = reducedRecords.Contains(r) ? $"{r.TotalWatts}W" : string.Empty,
                Color = SKColor.Parse("#42A5F5"),
                TextColor = SKColor.Parse("#1E88E5")
            }).ToArray();

        // 💎 Improved look: gradient and light theme
        EnergyHistoryChart = new LineChart
        {
            Entries = entries,
            LineMode = LineMode.Spline,         // Smooth curve
            LineSize = 5,
            LabelTextSize = 28,
            PointMode = PointMode.None,        // Remove circles for clean look
            BackgroundColor = SKColor.Parse("#F5F5F5"),
            LabelColor = SKColor.Parse("#444444"),

        };
    }


    [RelayCommand]
    private async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private void ClearHistory()
    {
        _historyService.ClearHistory();
        Application.Current?.MainPage?.DisplayAlert("Cleared", "Energy history has been reset.", "OK");
        LoadChart(); // Refresh immediately
    }
}
