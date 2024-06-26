using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
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
	private static MainViewModel _instance;
	public static MainViewModel Instance => _instance;

	public MainViewModel()
	{
		_instance = this;
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
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "Bezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "Farbe_FK", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		Tables.Add(tbl2);

		var tbl3 = new TableViewModel();
		tbl3.Name = "Geruch";
		tbl3.X = 300;
		tbl3.Y = 100;
		tbl3.Height = 200;
		tbl3.Width = 130;
		tbl3.Columns.Add(new ColumnViewModel(tbl3) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl3.Columns.Add(new ColumnViewModel(tbl3) { Name = "Geruchbezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		Tables.Add(tbl3);

		Tables.Add(new LineViewModel(tbl, tbl2));

		//string sql = SQLGenerator.Generate(Tables, DBMS.MSSQLServer);

		Tables.CollectionChanged += Tables_CollectionChanged;
	}

	private void Tables_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		if (e?.NewItems?.Count > 0 &&
			e.NewItems is IEnumerable addItems)
		{
			foreach (var item in addItems)
			{
				if (item is LineViewModel line)
				{
					line.CalculatePositions();
				}
			}
		}
	}

	public ObservableCollection<SelectableElement> Tables { get; } = [];

	[ObservableProperty]
	private SelectableElement _selectedTable;

	public SettingsViewModel Settings { get; } = new();

	[RelayCommand]
	private void DeleteItem(object item)
	{
		if (item is null)
		{
			return;
		}

		var result = MessageBox.Show("Do you want to delete the selected item?", "Delete item", MessageBoxButton.YesNo);
		if (result == MessageBoxResult.No)
		{
			return;
		}

		//Tables.Remove(item);
	}

	[RelayCommand]
	private void GenerateSQL()
	{
		string sql = Models.SQLGenerator.Generate(Tables.OfType<TableViewModel>(), Models.DBMS.MSSQLServer);
		MessageBox.Show(sql);
	}

	private void AddTable()
	{
		Tables.Add(new TableViewModel() { X = 200, Y = 100 });
	}
}
