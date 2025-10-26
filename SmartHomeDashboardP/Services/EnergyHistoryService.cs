using SmartHomeDashboardP.Models;
using System.Text.Json;

namespace SmartHomeDashboardP.Services;

public class EnergyHistoryService
{
    private const string HistoryKey = "energy_history";

    public void SaveHistory(List<EnergyRecord> records)
    {
        var json = JsonSerializer.Serialize(records);
        Preferences.Set(HistoryKey, json);
    }

    public List<EnergyRecord> LoadHistory()
    {
        if (!Preferences.ContainsKey(HistoryKey))
            return new List<EnergyRecord>();

        var json = Preferences.Get(HistoryKey, string.Empty);
        return string.IsNullOrEmpty(json)
            ? new List<EnergyRecord>()
            : JsonSerializer.Deserialize<List<EnergyRecord>>(json) ?? new List<EnergyRecord>();
    }

    public void ClearHistory()
    {
        Preferences.Remove(HistoryKey);
    }
}
