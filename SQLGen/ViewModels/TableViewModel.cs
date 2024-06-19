using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SQLGen.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class TableViewModel : SelectableElement
{
    [ObservableProperty]
    private string _name;

    public ObservableCollection<ColumnViewModel> Columns { get; } = [];

    public ObservableCollection<TableViewModel> ConnectedTo { get; } = [];

    //Visual Properties
    [ObservableProperty]
    private double _x;
    [ObservableProperty]
    private double _y;
    [ObservableProperty]
    private double _height;
    [ObservableProperty]
    private double _width;

    partial void OnXChanged(double oldValue, double newValue) => VisualPropertyChanged?.Invoke(this, this);
    partial void OnYChanged(double oldValue, double newValue) => VisualPropertyChanged?.Invoke(this, this);
    partial void OnHeightChanged(double oldValue, double newValue)
    {
        Debug.WriteLine($"Height: {oldValue} -> {newValue}");
        VisualPropertyChanged?.Invoke(this, this);
    }

    partial void OnWidthChanged(double oldValue, double newValue)
    {
        Debug.WriteLine($"Width: {oldValue} -> {newValue}");
        VisualPropertyChanged?.Invoke(this, this);
    }

    public event EventHandler<TableViewModel> VisualPropertyChanged;

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
        sql.AppendLine(string.Join($",{Environment.NewLine}", Columns.Select(x => $"\t{x.GenerateSQL(dbms)}")));
        sql.AppendLine($")");

        return sql.ToString();
    }

    internal RelativePosition CalculateRelativePosition(TableViewModel to)
    {
        double centerX1 = this.X + (this.Width / 2);
        double centerY1 = this.Y + (this.Height / 2);
        double centerX2 = to.X + (to.Width / 2);
        double centerY2 = to.Y + (to.Height / 2);

        double deltaX = centerX2 - centerX1;
        double deltaY = centerY2 - centerY1;

        double angleInDegrees = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

        return DegreeToRelativePosition(angleInDegrees);
    }

    internal Point GetPointOfSide(RelativePosition side)
    {
        return side switch
        {
            RelativePosition.Top => new Point((X + Width / 2), Y),
            RelativePosition.Right => new Point((X + Width), (Y + Height / 2)),
            RelativePosition.Bottom => new Point((X + Width / 2), Y + Height),
            RelativePosition.Left => new Point(X, Y + Height / 2),
            _ => throw new NotImplementedException($"Method {nameof(GetPointOfSide)} is not fully implemented"),
        };
    }

    private static RelativePosition DegreeToRelativePosition(double degree)
    {
        if (degree.IsBetween(45, 135)) return RelativePosition.Top;
        if (degree.IsBetween(135, 225)) return RelativePosition.Left;
        if (degree.IsBetween(225, 315)) return RelativePosition.Bottom;
        //if (degree.IsBetween(315, 45)) return RelativePosition.Top;
        return RelativePosition.Right;
    }
}
