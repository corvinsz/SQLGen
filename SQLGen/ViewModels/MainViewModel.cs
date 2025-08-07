using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SQLGen.Models;
using SQLGen.Services;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Linq;

namespace SQLGen.ViewModels;

public partial class MainViewModel : ObservableObject
{
	#region Demo-Data
	private void InitDemoData()
	{
		var tbl = new Table();
		tbl.Name = "Farbe";
		tbl.Name = "Author";
		tbl.X = 0;
		tbl.Y = 0;
		tbl.Height = 80;
		tbl.Width = 80;
		tbl.Columns.Add(new Column(tbl) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl.Columns.Add(new Column(tbl) { Name = "FirstName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl.Columns.Add(new Column(tbl) { Name = "LastName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl.Columns.Add(new Column(tbl) { Name = "Birthdate", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Decimal, Length = 16, Precision = 9 } });
		tbl.Columns.Add(new Column(tbl) { Name = "CreatedAt", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		Tables.Add(tbl);

		var tbl2 = new Table();
		tbl2.Name = "Book";
		tbl2.X = 100;
		tbl2.Y = 100;
		tbl2.Height = 90;
		tbl2.Width = 90;
		tbl2.Columns.Add(new Column(tbl2) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new Column(tbl2) { Name = "Author_FK", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tbl2.Columns.Add(new Column(tbl2) { Name = "Name", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tbl2.Columns.Add(new Column(tbl2) { Name = "ReleaseDate", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		Tables.Add(tbl2);

		Tables.Add(new Line(_settings, tbl, tbl2));
	}
	#endregion

	private readonly SettingsViewModel _settings;
	private readonly IDialogService _dialogService;

	public ISnackbarMessageQueue MessageQueue { get; }

	public MainViewModel(ISnackbarMessageQueue messageQueue,
						 SettingsViewModel settings,
						 IDialogService dialogService)
	{
		MessageQueue = messageQueue ?? throw new ArgumentNullException(nameof(messageQueue));
		_settings = settings;
		_dialogService = dialogService ?? throw new ArgumentNullException(nameof(messageQueue));
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
				if (item is Line line)
				{
					line.CalculateStartAndEndpoint();
				}
			}
		}
	}

	[RelayCommand]
	private async Task DeleteSelectedItem()
	{
		if (SelectedTable is null)
		{
			return;
		}

		var result = (MessageBoxResult?)await _dialogService.Show(new Views.Dialogs.MessageBoxDialog("Delete item", "Do you want to delete the selected item?", MessageBoxButton.YesNo), "RootDialog");
		if (result == MessageBoxResult.Yes)
		{
			if (SelectedTable is Table table)
			{
				table.DeleteConnections(Tables);
			}

			Tables.Remove(SelectedTable);
		}
	}

	[RelayCommand]
	private async Task AddTable()
	{
		var textInputControl = new Views.Dialogs.SimpleTextInputDialog(string.Empty, x => !string.IsNullOrWhiteSpace(x), "Table name");
		var result = await _dialogService.Show(textInputControl, "RootDialog");

		if (result is not string newTableName)
		{
			return;
		}

		var table = new Table
		{
			Name = newTableName,
			X = 300,
			Y = 300,
			Width = 200,
			Height = 200
		};
		Tables.Add(table);

		MessageQueue.Enqueue($"The table '{newTableName}' has successfully been added");

		if (_settings.WarnForDuplicates)
		{
			int existingTableCount = Tables.OfType<Table>().Count(x => string.Equals(x.Name, newTableName, StringComparison.InvariantCultureIgnoreCase));

			if (existingTableCount >= 2)
			{
				await ShowDuplicateTableDialog(table);
			}
		}
	}

	[RelayCommand]
	private async Task AddColumn(Table table)
	{
		var textInputControl = new Views.Dialogs.SimpleTextInputDialog(string.Empty, x => !string.IsNullOrWhiteSpace(x));
		var result = await _dialogService.Show(textInputControl, "RootDialog");

		if (result is not string newColumnName)
		{
			return;
		}

		var column = new Column(table);
		column.Name = newColumnName;

		// TODO
		//if (_settings.AutodetectKeys)
		//{
		//	column.PredictTypeAndKey();
		//}

		table.Columns.Add(column);

		// TODO
		//if (_settings.WarnForDuplicates)
		{
			int existingColumnCount = table.Columns.Count(x => string.Equals(x.Name, newColumnName, StringComparison.InvariantCultureIgnoreCase));

			if (existingColumnCount >= 2)
			{
				await ShowDuplicateColumnDialog(column);
			}
		}

		async Task ShowDuplicateColumnDialog(Column duplicateColumn)
		{
			string message = $"There are multiple columns with the name '{duplicateColumn.Name}'.";
			var duplicateWarningDialog = new Views.Dialogs.DuplicateWarningDialog(() => UndoAddColumn(duplicateColumn), message);
			await _dialogService.Show(duplicateWarningDialog, "RootDialog");
		}

		void UndoAddColumn(Column columnToRemove)
		{
			table.Columns.Remove(columnToRemove);
		}
	}

	[RelayCommand]
	private async Task Rename(INameable nameable)
	{
		var textInputControl = new Views.Dialogs.SimpleTextInputDialog(nameable.Name, x => !string.IsNullOrWhiteSpace(x));
		var result = await _dialogService.Show(textInputControl, "RootDialog");

		if (result is not string resultString)
		{
			return;
		}

		nameable.Name = resultString;
	}

	private async Task ShowDuplicateTableDialog(Table duplicateTable)
	{
		string message = $"There are multiple tables with the name '{duplicateTable.Name}'.";
		var duplicateWarningDialog = new Views.Dialogs.DuplicateWarningDialog(() => UndoAddTables(duplicateTable), message);
		await _dialogService.Show(duplicateWarningDialog, "RootDialog");
	}

	private void UndoAddTables(Table tableToRemove)
	{
		Tables.Remove(tableToRemove);
	}

	[RelayCommand]
	private async Task ShowExportDialog()
	{
		await _dialogService.Show(new Views.Dialogs.ExportDialog(), "RootDialog");
	}

	[RelayCommand]
	private async Task AddConnection(Table table)
	{
		var availableTables = Tables.WhereTablesNotConnectedToThis(table);

		var tableConnectorControl = new Views.Dialogs.TableConnectorDialog(availableTables);
		Table? result = await _dialogService.Show(tableConnectorControl, "RootDialog") as Table;

		if (result is null)
		{
			return;
		}

		Tables.Add(new Line(_settings, table, result));
	}

	private Lazy<ReleaseNotes.ReleaseNotesDialog> _releaseNotesDialog = new(() => new ReleaseNotes.ReleaseNotesDialog());
	[RelayCommand]
	private async Task ShowReleaseNotes()
	{
		//if (DialogHost.IsDialogOpen("RootDialog"))
		//{
		//	DialogHost.Close("RootDialog");
		//}
		await _dialogService.Show(_releaseNotesDialog.Value, "RootDialog");
	}
}
