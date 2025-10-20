using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeDashboardP.Models;

public class SmartDevice
{
    public string Name { get; set; } = string.Empty;
    public bool IsOn { get; set; }
    public string Icon { get; set; } = string.Empty;
    public string Status => IsOn ? "On" : "Off";
}
