using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class MainViewModel : ObservableObject
{
	public MainViewModel()
	{
		var tbl = new TableViewModel();
		tbl.Name = "Farbe";
		tbl.X = 0;
		tbl.Y = 0;
		tbl.Height = 80;
		tbl.Width = 80;
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Bezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Temperatur", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Decimal, Length = 16, Precision = 9 } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ErstelltAm", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		Tables.Add(tbl);

		var tbl2 = new TableViewModel();
		tbl2.Name = "Chemie";
		tbl2.X = 100;
		tbl2.Y = 100;
		tbl2.Height = 90;
		tbl2.Width = 90;
		tbl2.Columns.Add(new ColumnViewModel(tbl) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new ColumnViewModel(tbl) { Name = "Bezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl2.Columns.Add(new ColumnViewModel(tbl) { Name = "Farbe_FK", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.ConnectedTo.Add(tbl);
		Tables.Add(tbl2);

		Tables.Add(new LineViewModel(tbl, tbl2));

		//string sql = SQLGenerator.Generate(Tables, DBMS.MSSQLServer);
	}

	public ObservableCollection<SelectableElement> Tables { get; } = [];

	[ObservableProperty]
	private SelectableElement _selectedTable;

	[RelayCommand]
	private void DeleteSelectedItem()
	{
		if (SelectedTable is null)
		{
			return;
		}

		var result = MessageBox.Show("Do you want to delete the selected item?", "Delete item", MessageBoxButton.YesNo);
		if (result == MessageBoxResult.No)
		{
			return;
		}

		Tables.Remove(SelectedTable);
		SelectedTable = null;
	}

	private void AddTable()
	{
		Tables.Add(new TableViewModel() { X = 200, Y = 100 });
	}
}
