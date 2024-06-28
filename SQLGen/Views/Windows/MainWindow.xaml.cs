using MaterialDesignThemes.Wpf;
using SQLGen.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLGen.Windows;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	private MainViewModel _viewModel;
	private Panel _itemsPanel;

	public MainWindow()
	{
		InitializeComponent();
		_viewModel = (MainViewModel)DataContext;
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

	//private void TableControl_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
	//{
	//	_viewModel.SelectedTable = null;
	//}

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

	private void Line_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
	{
		MessageBox.Show("line clicked");
	}

	private async void Settings_Click(object sender, RoutedEventArgs e)
	{
		var settingsDialog = new Views.Controls.SettingsControl();
		await DialogHost.Show(settingsDialog, "RootDialog");
	}
}