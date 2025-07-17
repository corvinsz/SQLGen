using SQLGen.Models;
using System.Windows.Controls;
using System.Windows.Input;

namespace SQLGen.Views.Dialogs;
/// <summary>
/// Interaction logic for TableConnectorControl.xaml
/// </summary>
public partial class TableConnectorDialog : UserControl
{
	public TableConnectorDialog(IEnumerable<Table> availableTables)
	{
		InitializeComponent();
		lbTables.ItemsSource = availableTables;
	}

	private void lbTables_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
	{
		//DialogHost.CloseDialogCommand.Execute(lbTables.SelectedItem);
	}
}
