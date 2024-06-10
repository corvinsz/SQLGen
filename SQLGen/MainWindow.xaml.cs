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

namespace SQLGen;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainViewModel _viewModel;
    private UIElement _selectedTableControl;
    public MainWindow()
    {
        InitializeComponent();
        _viewModel = (MainViewModel)DataContext;
    }

    private void TableControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        int? index = ((FrameworkElement)sender).Tag as int?;

        if (index is null)
        {
            return;
        }

        _viewModel.SelectedTable = _viewModel.Tables[index.Value];
        _selectedTableControl = (UIElement)sender;
        _selectedTableControl?.CaptureMouse();
    }

    private void TableControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_selectedTableControl is not null)
        {
            _selectedTableControl.ReleaseMouseCapture();
        }

        _viewModel.SelectedTable = null;
    }
}