using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel; 

namespace SmartHomeDashboardP.Models;

public partial class SmartDevice : ObservableObject
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool isOn;

    [ObservableProperty]
    private string icon = string.Empty;

    [ObservableProperty]
    private string status = string.Empty;

    public IRelayCommand? ToggleCommand { get; set; }
}
