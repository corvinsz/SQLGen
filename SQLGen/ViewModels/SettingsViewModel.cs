using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.ViewModels;
public partial class SettingsViewModel : ObservableObject
{
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
		foreach (var line in MainViewModel.Instance.Tables.OfType<LineViewModel>())
		{
			line.StrokeThickness = value;
		}
	}
}
