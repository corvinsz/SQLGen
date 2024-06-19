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
        //this.Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        _itemsPanel = Models.XAMLHelper.FindChild<Canvas>(icTables);
        DrawTableConnection();
    }

    private void DrawTableConnection()
    {
        foreach (TableViewModel table in _viewModel.Tables)
        {
            if (table.ConnectedTo.Count < 1)
            {
                continue;
            }
            foreach (var table2 in table.ConnectedTo)
            {
                Line line = new Line
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2,
                    X1 = table.X,
                    Y1 = table.Y,
                    X2 = table2.X,
                    Y2 = table2.Y
                };

                // Add the line to the Canvas
                _itemsPanel.Children.Add(line);
            }
        }
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

    private void TableControl_PreviewMouseLeftButtonUp(object sender, MouseEventArgs e)
    {
        _viewModel.SelectedTable = null;
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

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        _itemsPanel = Models.XAMLHelper.FindChild<Canvas>(icTables);
        DrawTableConnection();
    }
}