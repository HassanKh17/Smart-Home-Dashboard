using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeDashboardP.Models;
public class EnergyRecord
{
    public DateTime Timestamp { get; set; }       // when the reading was taken
    public int TotalWatts { get; set; }           // total energy usage at that moment
}
