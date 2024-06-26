using MaterialDesignThemes.Wpf;
using SQLGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLGen.Views.Controls;
/// <summary>
/// Interaction logic for TableConnectorControl.xaml
/// </summary>
public partial class TableConnectorControl : UserControl
{
	public TableConnectorControl(IEnumerable<TableViewModel> availableTables)
	{
		InitializeComponent();
		lbTables.ItemsSource = availableTables;
	}

	private void lbTables_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		//DialogHost.CloseDialogCommand.Execute(lbTables.SelectedItem);
	}
}
