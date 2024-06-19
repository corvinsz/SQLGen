using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLGen;

public abstract partial class SelectableElement : ObservableObject
{
    [ObservableProperty]
    private bool _isSelected;

    public Type SelfType => this.GetType();
}

public enum RelativePosition
{
    //TopLeft,
    Top,
    //TopRight,
    Right,
    //BottomRight,
    Bottom,
    //BottomLeft,
    Left
}

public partial class LineViewModel : SelectableElement
{
    private readonly TableViewModel _from;
    private readonly TableViewModel _to;

    public LineViewModel(TableViewModel from, TableViewModel to)
    {
        ArgumentNullException.ThrowIfNull(from, nameof(from));
        ArgumentNullException.ThrowIfNull(to, nameof(to));

        Stroke = Brushes.Fuchsia;
        StrokeThickness = 4;
        _from = from;
        _to = to;

        _from.VisualPropertyChanged += Table_VisualPropertyChanged;
        _to.VisualPropertyChanged += Table_VisualPropertyChanged;
    }

    private void Table_VisualPropertyChanged(object? sender, TableViewModel e)
    {
        CalculatePositions();
    }

    private void CalculatePositions()
    {
        RelativePosition pos = _from.CalculateRelativePosition(_to);
        System.Windows.Point P1 = _from.GetPointOfSide(pos);
        X1 = P1.X;
        Y1 = P1.Y;

        pos = pos.GetOppositeSide();
        System.Windows.Point P2 = _to.GetPointOfSide(pos);
        X2 = P2.X;
        Y2 = P2.Y;
    }

    [ObservableProperty]
    private double _x1;
    [ObservableProperty]
    private double _y1;
    [ObservableProperty]
    private double _x2;
    [ObservableProperty]
    private double _y2;

    [ObservableProperty]
    private Brush _stroke;

    [ObservableProperty]
    private double _strokeThickness;
}



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
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Bezeichnung", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "Temperatur", DataType = new SqlDataType() { Type = System.Data.SqlDbType.Decimal, Length = 16, Precision = 9 } });
        tbl.Columns.Add(new ColumnViewModel(tbl) { Name = "ErstelltAm", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
        Tables.Add(tbl);

        var tbl2 = new TableViewModel();
        tbl2.Name = "Chemie";
        tbl2.X = 400;
        tbl2.Y = 100;
        tbl2.Height = 300;
        tbl2.Width = 150;
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

    private void AddTable()
    {
        Tables.Add(new TableViewModel() { X = 200, Y = 100 });
    }
}
