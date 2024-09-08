using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SQLGen.Helpers;
using SQLGen.SQLGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class ExportViewModel : ObservableObject
{
    private readonly IMessageService<SnackbarMessageQueue> _messageService;
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

    public ExportViewModel(IMessageService<SnackbarMessageQueue> messageService, MainViewModel mainViewModel)
    {
        _messageService = messageService;
        _tables = mainViewModel.Tables;
        SQLGenerators = GetSQLProviders().ToList();
    }

    private IEnumerable<ISQLGenerator> GetSQLProviders()
    {
        // Get all types in the assembly
        Type[] typesInAssembly = Assembly.GetExecutingAssembly().GetTypes();

        // Find all classes that implement the ISQLGenerator interface
        var sqlGeneratorTypes = typesInAssembly.Where(t => typeof(ISQLGenerator).IsAssignableFrom(t) && t.IsClass);

        // Instantiate the classes
        foreach (Type type in sqlGeneratorTypes)
        {
            ISQLGenerator sqlGenerator = (ISQLGenerator)Activator.CreateInstance(type)!;
            yield return sqlGenerator;
        }
    }



    [RelayCommand]
    private void CopyQuery()
    {
        if (String.IsNullOrEmpty(Query))
        {
            return;
        }
        Clipboard.SetText(Query);
        _messageService.ShowMessage("Query copied to clipboard");
    }
}
