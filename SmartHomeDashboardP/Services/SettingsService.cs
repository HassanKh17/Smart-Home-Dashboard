using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeDashboardP.Models;

namespace SmartHomeDashboardP.Services;

public class SettingsService
{
    private const string KeyUpdateInterval = "update_interval";
    private const string KeyDarkTheme = "is_dark_theme";
    private const string KeySimulation = "enable_simulation";

    public AppSettings LoadSettings()
    {
        return new AppSettings
        {
            UpdateIntervalSeconds = Preferences.Get(KeyUpdateInterval, 3),
            IsDarkTheme = Preferences.Get(KeyDarkTheme, false),
            EnableSimulation = Preferences.Get(KeySimulation, true)
        };
    }

    public void SaveSettings(AppSettings settings)
    {
        Preferences.Set(KeyUpdateInterval, settings.UpdateIntervalSeconds);
        Preferences.Set(KeyDarkTheme, settings.IsDarkTheme);
        Preferences.Set(KeySimulation, settings.EnableSimulation);
    }
}
