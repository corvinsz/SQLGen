using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SQLGen.Helpers;
using SQLGen.Models;
using SQLGen.ViewModels;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SQLGen.Windows;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private readonly MainViewModel _viewModel;

	public MainWindow()
	{
		InitializeComponent();
		//_viewModel = new MainViewModel(new SnackbarMessageService(mainSnackbar.MessageQueue));
		_viewModel = App.ServiceProvider.GetService<MainViewModel>();
		this.DataContext = _viewModel;
	}

	private void TableControl_PreviewMouseLeftButtonDown(object sender, MouseEventArgs e)
	{
		int? index = ((FrameworkElement)sender).Tag as int?;

		if (index is null)
		{
			return;
		}

		_viewModel.SelectedTable = _viewModel.Tables[index.Value];
	}

	private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
	{
		if (_viewModel?.SelectedTable is not TableViewModel tbl)
		{
			return;
		}

		tbl.X += e.HorizontalChange;
		tbl.Y += e.VerticalChange;
		e.Handled = true;
	}

	private async void Settings_Click(object sender, RoutedEventArgs e)
	{
		var settingsDialog = new Views.Dialogs.SettingsDialog();
		await DialogHost.Show(settingsDialog, "RootDialog");
		var settings = App.ServiceProvider.GetRequiredService<SettingsViewModel>();
		await settings.SaveAsync();
	}

	private void btnResizeTables_Click(object sender, RoutedEventArgs e)
	{
		var tables = Models.XAMLHelper.FindVisualChildren<Views.Controls.TableControl>(this);

		if (tables is null || tables.IsEmpty())
		{
			return;
		}

		int resizedTablesCount = 0;
		foreach (Views.Controls.TableControl table in tables)
		{
			bool didResize = table.DoAutoResize();
			if (didResize)
			{
				resizedTablesCount++;
			}
		}

		var messageService = App.ServiceProvider.GetRequiredService<IMessageService<SnackbarMessageQueue>>();

		if (resizedTablesCount == 0)
		{
			messageService.ShowMessage($"All tables are already sized accordingly");
		}
		else
		{
			messageService.ShowMessage($"Successfully resized {resizedTablesCount} tables");
		}
	}
}