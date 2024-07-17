using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SQLGen.SQLGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class ExportViewModel : ObservableObject
{
	private readonly IEnumerable<SelectableElement> _tables;

	public IReadOnlyList<ISQLGenerator> SQLGenerators { get; }

	[ObservableProperty]
	private ISQLGenerator _selectedGenerator;

	partial void OnSelectedGeneratorChanged(ISQLGenerator value)
	{
		Query = value?.Generate(_tables.OfType<TableViewModel>());
	}

	[ObservableProperty]
	private string _query;

	public ExportViewModel(IEnumerable<SelectableElement> tables)
	{
		_tables = tables;
		SQLGenerators = new List<ISQLGenerator>()
		{
			new MSSQLServerGenerator(),
			new MySQLGenerator()
		};
	}

	[RelayCommand]
	private async Task ShowExportDialog()
	{
		await DialogHost.Show(new Views.Dialogs.ExportDialog(), "RootDialog");
	}

	[RelayCommand]
	private void CopyQuery()
	{
		if (String.IsNullOrEmpty(Query))
		{
			return;
		}
		Clipboard.SetText(Query);
		MainViewModel.Instance.MessageService.ShowMessage("Query copied to clipboard");
	}
}
