using MaterialDesignThemes.Wpf;
using SQLGen.Models;
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
    private readonly MainViewModel _viewModel;

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

        if (resizedTablesCount == 0)
        {
            mainSnackbar.MessageQueue.Enqueue($"All tables are already sized accordingly");
        }
        else
        {
            mainSnackbar.MessageQueue.Enqueue($"Successfully resized {resizedTablesCount} tables");
        }
    }
}