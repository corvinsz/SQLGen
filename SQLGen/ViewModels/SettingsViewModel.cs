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
	private int _positionSnapping = 20;

	[ObservableProperty]
	private int _sizeSnapping = 20;
}
