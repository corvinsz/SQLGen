using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SQLGen.Models;
using SQLGen.SQLGenerator;
using System.Reflection;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class ExportViewModel : ObservableObject
{
	private readonly IEnumerable<SelectableElement> _tables;
	private readonly ISnackbarMessageQueue _messageQueue;

	public IReadOnlyList<ISQLGenerator> SQLGenerators { get; }

	[ObservableProperty]
	private ISQLGenerator _selectedGenerator;

	partial void OnSelectedGeneratorChanged(ISQLGenerator value)
	{
		Query = value?.Generate(_tables.OfType<Table>());
	}

	[ObservableProperty]
	private string _query;

	public ExportViewModel(ISnackbarMessageQueue messageQueue, MainViewModel mainViewModel)
	{
		_messageQueue = messageQueue;
		_tables = mainViewModel.Tables;
		SQLGenerators = GetSQLProviders().ToList();
	}

	private IEnumerable<ISQLGenerator> GetSQLProviders()
	{
		Type[] typesInAssembly = Assembly.GetExecutingAssembly().GetTypes();

		var sqlGeneratorTypes = typesInAssembly.Where(t => typeof(ISQLGenerator).IsAssignableFrom(t) && t.IsClass);

		foreach (Type type in sqlGeneratorTypes)
		{
			ISQLGenerator sqlGenerator = (ISQLGenerator)Activator.CreateInstance(type)!;
			yield return sqlGenerator;
		}
	}

	[RelayCommand]
	private void CopyQuery()
	{
		if (string.IsNullOrEmpty(Query))
		{
			return;
		}
		Clipboard.SetText(Query);
		_messageQueue.Enqueue("Query copied to clipboard");
	}
}
