using Microcharts.Maui;
using Microsoft.Maui.Controls;
using SmartHomeDashboardP.Models;
using SmartHomeDashboardP.ViewModels;

namespace SmartHomeDashboardP.Views;

public partial class EnergyHistoryPage : ContentPage
{
    private double _chartWidth;
    private IList<EnergyRecord> _records = new List<EnergyRecord>();

    public EnergyHistoryPage()
    {
        InitializeComponent();

        var vm = BindingContext as EnergyHistoryViewModel;
        if (vm != null)
        {
            // Load data for tooltip
            var service = new SmartHomeDashboardP.Services.EnergyHistoryService();
            _records = service.LoadHistory();
        }

        HistoryChartView.SizeChanged += (s, e) => _chartWidth = HistoryChartView.Width;

        // Tap gesture
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnChartTapped;
        HistoryChartView.GestureRecognizers.Add(tapGesture);

        // Pan (drag) gesture
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnChartPanned;
        HistoryChartView.GestureRecognizers.Add(panGesture);
    }

    private void OnChartTapped(object sender, TappedEventArgs e)
    {
        var position = e.GetPosition((View)sender);
        if (position.HasValue)
            ShowTooltip(position.Value.X);
    }

    private void OnChartPanned(object sender, PanUpdatedEventArgs e)
    {
        if (e.StatusType == GestureStatus.Running)
            ShowTooltip(e.TotalX);
        else if (e.StatusType == GestureStatus.Completed)
            TooltipFrame.IsVisible = false;
    }

    private void ShowTooltip(double xPosition)
    {
        if (_records.Count == 0 || _chartWidth == 0)
            return;

        int index = (int)Math.Clamp((_records.Count - 1) * (xPosition / _chartWidth), 0, _records.Count - 1);
        var record = _records[index];

        TooltipLabel.Text = $"{record.Timestamp:HH:mm:ss}\n{record.TotalWatts} W";
        // 🧮 Calculate relative Y position based on min/max values
        float minValue = _records.Min(r => r.TotalWatts);
        float maxValue = _records.Max(r => r.TotalWatts);

        if (Math.Abs(maxValue - minValue) < 0.1f)
            maxValue += 1;

        // Actual chart height (excluding visual padding)
        double fullHeight = HistoryChartView.Height;
        double chartPaddingTop = fullHeight * 0.10;   // ~10% padding from top
        double chartPaddingBottom = fullHeight * 0.15; // ~15% bottom margin
        double drawableHeight = fullHeight - (chartPaddingTop + chartPaddingBottom);

        // Map current value to drawable height (invert so higher values are higher on screen)
        double normalized = (record.TotalWatts - minValue) / (maxValue - minValue);
        double invertedY = chartPaddingTop + (1 - normalized) * drawableHeight;

        // Tooltip position (above the point)
        double tooltipX = Math.Clamp(xPosition - (TooltipFrame.Width / 2), 0, _chartWidth - TooltipFrame.Width);
        double tooltipY;
        bool showBelow = invertedY < 70;
        if (showBelow)
            tooltipY = invertedY + 20; // below point
        else
            tooltipY = invertedY - TooltipFrame.Height - 20; // above point

        // Apply new positions
        AbsoluteLayout.SetLayoutBounds(TooltipFrame, new Rect(tooltipX, tooltipY, -1, -1));
        AbsoluteLayout.SetLayoutBounds(CursorDot, new Rect(xPosition - 4, invertedY - 4, 8, 8));

        // Fade in if needed
        if (!TooltipFrame.IsVisible)
        {
            TooltipFrame.Opacity = 0;
            CursorDot.Opacity = 0;
            TooltipFrame.IsVisible = true;
            CursorDot.IsVisible = false;

            TooltipFrame.FadeTo(1, 150, Easing.CubicInOut);
            CursorDot.FadeTo(1, 150, Easing.CubicInOut);
        }
    }
}
