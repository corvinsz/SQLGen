using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SQLGen.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
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

    private double RoundToNearestTen(double value)
    {
        double snappingDistance = (double)MainViewModel.Instance.Settings.SnapDistance;

        return Math.Round(value / snappingDistance) * snappingDistance;
    }

    partial void OnXChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
        _x = RoundToNearestTen(value);
        VisualPropertyChanged?.Invoke(this, this);
    }
    partial void OnYChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
        _y = RoundToNearestTen(value);
        VisualPropertyChanged?.Invoke(this, this);
    }
    partial void OnHeightChanged(double oldValue, double newValue)
    {
        VisualPropertyChanged?.Invoke(this, this);
    }

    partial void OnWidthChanged(double oldValue, double newValue)
    {
        VisualPropertyChanged?.Invoke(this, this);
    }

    public event EventHandler<TableViewModel> VisualPropertyChanged;

    [RelayCommand]
    private async Task AddColumn()
    {
        var settingsDialog = new Views.Controls.SimpleTextInputControl(string.Empty, x => !string.IsNullOrWhiteSpace(x));
        var result = await DialogHost.Show(settingsDialog, "RootDialog");

        if (result is not string resultString)
        {
            return;
        }

        Columns.Add(new ColumnViewModel(this) { Name = resultString });
    }

    [RelayCommand]
    private async Task Rename()
    {
        var txtInputControl = new Views.Controls.SimpleTextInputControl(Name, x => !string.IsNullOrWhiteSpace(x));
        var result = await DialogHost.Show(txtInputControl, "RootDialog");

        if (result is not string resultString)
        {
            return;
        }

        Name = resultString;
    }

    [RelayCommand]
    private async Task AddConnection()
    {
        var tableConnectorControl = new Views.Controls.TableConnectorControl();
        var result = await DialogHost.Show(tableConnectorControl, "RootDialog");

        if (result is not TableViewModel selectedTable)
        {
            return;
        }

        ConnectedTo.Add(selectedTable);
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

        // Invert deltaY to account for the top-left origin of the WPF coordinate system
        deltaY = -deltaY;

        //Calculate angle
        double angleInDegrees = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

        //Convert angle to enum
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

    private static RelativePosition DegreeToRelativePosition(double angle)
    {
        angle = NormalizeAngle(angle);

        if (angle.IsBetween(45, 135)) return RelativePosition.Top;
        if (angle.IsBetween(135, 225)) return RelativePosition.Left;
        if (angle.IsBetween(225, 315)) return RelativePosition.Bottom;
        return RelativePosition.Right;
    }

    private static double NormalizeAngle(double angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }

    [RelayCommand]
    private void ConnectTo(TableViewModel tableViewModel)
    {
        if (ConnectedTo.Contains(tableViewModel) == false)
        {
            ConnectedTo.Add(tableViewModel);
        }
    }
}
