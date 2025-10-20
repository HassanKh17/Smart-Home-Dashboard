using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace SmartHomeDashboardP.Models;

public class SmartDevice
{
    public string Name { get; set; } = string.Empty;
    public bool IsOn { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;

    public IRelayCommand? ToggleCommand { get; set; }
}
