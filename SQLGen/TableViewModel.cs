using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text;

namespace SQLGen;

public partial class TableViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    public ObservableCollection<ColumnViewModel> Columns { get; } = [];

    //Visual Properties
    [ObservableProperty]
    private double _x;
    [ObservableProperty]
    private double _y;
    [ObservableProperty]
    private double _height;
    [ObservableProperty]
    private double _width;

    [RelayCommand]
    private void AddColumn()
    {
        Columns.Add(new ColumnViewModel(this) { Name = "ErstelltAm" });
    }

    internal string? GenerateSQL(DBMS dbms)
    {
        if (Columns is null || Columns.Count == 0)
        {
            return string.Empty;
        }

        StringBuilder sql = new();
        sql.AppendLine($"create table {Name} (");
        foreach (ColumnViewModel column in Columns)
        {
            sql.AppendLine(column.GenerateSQL(dbms));
        }
        sql.AppendLine($")");
    }
}
