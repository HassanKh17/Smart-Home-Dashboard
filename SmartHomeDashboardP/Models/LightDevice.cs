using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeDashboardP.Models;

public class LightDevice : SmartDevice
{
    public int Brightness { get; set; } = 50; // 0–100%
}

