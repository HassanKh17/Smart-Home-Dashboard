using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeDashboardP.Models;

public class LockDevice : SmartDevice
{
    public bool IsLocked { get; set; } = true;
    public string LockStatus => IsLocked ? "Locked" : "Unlocked";
}
