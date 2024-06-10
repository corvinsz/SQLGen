using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Columns.Add(new ColumnViewModel() { Name = "ErstelltAm" });
    }
}

public partial class ColumnViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isPrimaryKey;
    [ObservableProperty]
    private bool _isForeignKey;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private SqlDataType _dataType;
}

public class SqlDataType
{
}