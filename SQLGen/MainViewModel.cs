using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        var tbl = new TableViewModel();
        tbl.Name = "Farbe";
        tbl.X = 200;
        tbl.Y = 100;
        tbl.Height = 300;
        tbl.Width = 150;
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Bezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar } });
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ErstelltAm", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
        Tables.Add(tbl);
    }

    public ObservableCollection<TableViewModel> Tables { get; } = [];

    [ObservableProperty]
    private TableViewModel _selectedTable;

    private void AddTable()
    {
        Tables.Add(new TableViewModel() { X = 200, Y = 100 });
    }
}
