using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SQLGen.Models;
using SQLGen.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velopack;
using Velopack.Sources;

namespace SQLGen.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
	private const string DEFAULT_CONFIG_FILE = "defaultSettings.json";
	private readonly string _configFile;
	private readonly ISnackbarMessageQueue _messageQueue;
	private readonly IErrorHandler _errorHandler;

	public SettingsViewModel(string configFile,
							 ISnackbarMessageQueue messageQueue,
							 IErrorHandler errorHandler)
	{
		_configFile = configFile;
		_messageQueue = messageQueue;
		_errorHandler = errorHandler;
		ReadConfig();

		const string repoUrl = "https://github.com/corvinsz/SQLGen";
		_um = new UpdateManager(new GithubSource(repoUrl, "", true));
		CurrentAppVersion = _um.CurrentVersion?.ToString() ?? "n.a.";
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
		// TODO
		//var mv = App.ServiceProvider.GetRequiredService<MainViewModel>();
		//foreach (var line in mv.Tables.OfType<Line>())
		//{
		//	line.StrokeThickness = value;
		//}
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

	#region velopack_stuff
	private readonly UpdateManager _um;
	private UpdateInfo? _update;

	public bool IsUpdateAvailable => _update is not null;

	[ObservableProperty]
	private int _downloadProgress = 0;

	[ObservableProperty]
	private string _currentAppVersion;

	[RelayCommand]
	private async Task CheckForUpdatesUpdate()
	{
		if (!_um.IsInstalled)
		{
			_messageQueue.Enqueue("App is not installed");
			return;
		}

		try
		{
			_update = await _um.CheckForUpdatesAsync().ConfigureAwait(true);
			OnPropertyChanged(nameof(IsUpdateAvailable));

			if (IsUpdateAvailable)
			{
				_messageQueue.Enqueue($"Update available: {_update?.TargetFullRelease.Version.ToString()}");
			}
			else
			{
				_messageQueue.Enqueue("No updates available");
			}
		}
		catch (Exception ex)
		{
			_errorHandler.HandleError(ex);
		}
	}

	[RelayCommand]
	private async Task ApplyUpdateAndRestart()
	{
		if (_update is null)
		{
			return;
		}

		try
		{
			await _um.DownloadUpdatesAsync(_update, (progress) => DownloadProgress = progress).ConfigureAwait(true);
			_um.ApplyUpdatesAndRestart(_update);
		}
		catch (Exception ex)
		{
			_errorHandler.HandleError(ex);
		}
	}
	#endregion
}
