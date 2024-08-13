using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
    private const string DEFAULT_CONFIG_FILE = "defaultSettings.json";
    private readonly string _configFile;

    public SettingsViewModel(string configFile)
    {
        _configFile = configFile;
        ReadConfig();
    }

    [ObservableProperty]
    private int _positionRounding = 20;

    [ObservableProperty]
    private int _sizeRounding = 20;

    [ObservableProperty]
    private int _lineThickness = 2;

    [ObservableProperty]
    private bool _autodetectKeys = true;

    [ObservableProperty]
    private bool _isDarkModeEnabled = true;

    [ObservableProperty]
    private bool _warnForDuplicates = true;

    partial void OnIsDarkModeEnabledChanged(bool value)
    {
        Helpers.ThemeHelper.ToggleTheme(value);
    }

    partial void OnLineThicknessChanged(int value)
    {
        var mv = App.ServiceProvider.GetRequiredService<MainViewModel>();
        foreach (var line in mv.Tables.OfType<LineViewModel>())
        {
            line.StrokeThickness = value;
        }
    }

    public async Task SaveAsync()
    {
        string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented);
        await File.WriteAllTextAsync(_configFile, jsonString);
    }

    public async Task ReadConfigAsync()
    {
        if (File.Exists(_configFile))
        {
            string jsonString = await File.ReadAllTextAsync(_configFile);
            JsonConvert.PopulateObject(jsonString, this);
        }
    }

    public void ReadConfig()
    {
        if (File.Exists(_configFile))
        {
            string jsonString = File.ReadAllText(_configFile);
            JsonConvert.PopulateObject(jsonString, this);
        }
    }

    [RelayCommand]
    private async Task RestoreDefaultsAsync()
    {
        if (File.Exists(DEFAULT_CONFIG_FILE))
        {
            File.Copy(DEFAULT_CONFIG_FILE, _configFile, true);
            await ReadConfigAsync();
        }
    }
}
