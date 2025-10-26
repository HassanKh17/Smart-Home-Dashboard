using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeDashboardP.Models;

public class AppSettings
{
    public int UpdateIntervalSeconds { get; set; } = 3;
    public bool IsDarkTheme { get; set; } = false;
    public bool EnableSimulation { get; set; } = true;
}
