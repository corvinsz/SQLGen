using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class MainViewModel : ObservableObject
{
	#region Demo-Data
	private void InitDemoData()
	{
		var tbl = App.ServiceProvider.GetRequiredService<TableViewModel>();
		tbl.Name = "Farbe";
		tbl.Name = "Author";
		tbl.X = 0;
		tbl.Y = 0;
		tbl.Height = 80;
		tbl.Width = 80;
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "FirstName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "LastName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Birthdate", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Decimal, Length = 16, Precision = 9 } });
		tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "CreatedAt", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		Tables.Add(tbl);

		var tbl2 = App.ServiceProvider.GetRequiredService<TableViewModel>();
		tbl2.Name = "Book";
		tbl2.X = 100;
		tbl2.Y = 100;
		tbl2.Height = 90;
		tbl2.Width = 90;
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "Author_FK", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "Name", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl2.Columns.Add(new ColumnViewModel(tbl2) { Name = "ReleaseDate", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		Tables.Add(tbl2);

		Tables.Add(new LineViewModel(_settings, tbl, tbl2));
	}
	#endregion

	private readonly SettingsViewModel _settings;

	public MainViewModel(SettingsViewModel settings)
	{
		_settings = settings;

		InitDemoData();

		Tables.CollectionChanged += Tables_CollectionChanged;
	}

	public ObservableCollection<SelectableElement> Tables { get; } = [];

	[ObservableProperty]
	private SelectableElement _selectedTable;

	partial void OnSelectedTableChanged(SelectableElement? oldValue, SelectableElement newValue)
	{
		if (oldValue is not null)
		{
			oldValue.IsSelected = false;
		}

		if (newValue is not null)
		{
			newValue.IsSelected = true;
		}
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
					line.CalculateStartAndEndpoint();
				}
			}
		}
	}

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

		if (SelectedTable is TableViewModel table)
		{
			table.DeleteConnections(Tables);
		}

		Tables.Remove(SelectedTable);
	}

	[RelayCommand]
	private async Task AddTable()
	{
		var textInputControl = new Views.Dialogs.SimpleTextInputDialog(string.Empty, x => !string.IsNullOrWhiteSpace(x));
		var result = await DialogHost.Show(textInputControl, "RootDialog");

		if (result is not string newTableName)
		{
			return;
		}

		var table = App.ServiceProvider.GetRequiredService<TableViewModel>();
		table.Name = newTableName;
		table.X = 300;
		table.Y = 300;
		table.Width = 200;
		table.Height = 200;
		Tables.Add(table);

		if (_settings.WarnForDuplicates)
		{
			int existingTableCount = Tables.OfType<TableViewModel>().Count(x => string.Equals(x.Name, newTableName, StringComparison.InvariantCultureIgnoreCase));

			if (existingTableCount >= 2)
			{
				await ShowDuplicateTableDialog(table);
			}
		}
	}

	private async Task ShowDuplicateTableDialog(TableViewModel duplicateTable)
	{
		string message = $"There are multiple tables with the name '{duplicateTable.Name}'.";
		var duplicateWarningDialog = new Views.Dialogs.DuplicateWarningDialog(() => UndoAddTables(duplicateTable), message);
		await DialogHost.Show(duplicateWarningDialog, "RootDialog");
	}

	private void UndoAddTables(TableViewModel tableToRemove)
	{
		Tables.Remove(tableToRemove);
	}

	[RelayCommand]
	private async Task ShowExportDialog()
	{
		await DialogHost.Show(new Views.Dialogs.ExportDialog(), "RootDialog");
	}
}
