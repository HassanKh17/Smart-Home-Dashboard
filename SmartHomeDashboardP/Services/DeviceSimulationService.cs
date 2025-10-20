using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeDashboardP.Models;
using System.Timers;
using System.Collections.ObjectModel;

namespace SmartHomeDashboardP.Services;

public class DeviceSimulationService
{
    public ObservableCollection<SmartDevice> Devices { get; set; }
    private readonly System.Timers.Timer _timer;


    public DeviceSimulationService()
    {
        Devices = new ObservableCollection<SmartDevice>
        {
            new LightDevice { Name = "Living Room Light", Icon = "💡", IsOn = true, Brightness = 70 },
            new ThermostatDevice { Name = "Thermostat", Icon = "🌡️", IsOn = true, Temperature = 22.5 },
            new LockDevice { Name = "Front Door Lock", Icon = "🔒", IsOn = true, IsLocked = true }
        };

        _timer = new System.Timers.Timer(3000); // every 3 seconds
        _timer.Elapsed += (s, e) => SimulateChanges();
        _timer.Start();
    }

    private void SimulateChanges()
    {
        foreach (var device in Devices.OfType<ThermostatDevice>())
        {
            // small random fluctuation
            device.Temperature += (new Random().NextDouble() - 0.5);
        }
    }
}
