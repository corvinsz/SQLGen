﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SQLGen.Helpers;
using SQLGen.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Text;
using System.Windows;

namespace SQLGen.ViewModels;

public partial class TableViewModel : SelectableElement
{
    private readonly SettingsViewModel _settings;
    public TableViewModel(SettingsViewModel settings)
    {
        _settings = settings;
    }

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

    partial void OnXChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        _x = Helpers.MathHelper.RoundToNearestValue(value, _settings.PositionRounding);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        VisualPropertyChanged?.Invoke(this, this);
    }
    partial void OnYChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        _y = Helpers.MathHelper.RoundToNearestValue(value, _settings.PositionRounding);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        VisualPropertyChanged?.Invoke(this, this);
    }
    partial void OnHeightChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        _height = Helpers.MathHelper.RoundToNextUpperInterval(value, _settings.SizeRounding);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        VisualPropertyChanged?.Invoke(this, this);
    }

    partial void OnWidthChanged(double value)
    {
        //Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        _width = Helpers.MathHelper.RoundToNextUpperInterval(value, _settings.SizeRounding);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
        VisualPropertyChanged?.Invoke(this, this);
    }

    public event EventHandler<TableViewModel> VisualPropertyChanged;

    [RelayCommand]
    private async Task AddColumn()
    {
        var textInputControl = new Views.Dialogs.SimpleTextInputDialog(string.Empty, x => !string.IsNullOrWhiteSpace(x));
        var result = await DialogHost.Show(textInputControl, "RootDialog");

        if (result is not string newColumnName)
        {
            return;
        }

        var column = new ColumnViewModel(this);
        column.Name = newColumnName;

        if (_settings.AutodetectKeys)
        {
            column.PredictTypeAndKey();
        }

        Columns.Add(column);

        if (_settings.WarnForDuplicates)
        {
            int existingColumnCount = Columns.Count(x => string.Equals(x.Name, newColumnName, StringComparison.InvariantCultureIgnoreCase));

            if (existingColumnCount >= 2)
            {
                await ShowDuplicateColumnDialog(column);
            }
        }
    }

    private async Task ShowDuplicateColumnDialog(ColumnViewModel duplicateColumn)
    {
        string message = $"There are multiple columns with the name '{duplicateColumn.Name}'.";
        var duplicateWarningDialog = new Views.Dialogs.DuplicateWarningDialog(() => UndoAddColumn(duplicateColumn), message);
        await DialogHost.Show(duplicateWarningDialog, "RootDialog");
    }

    private void UndoAddColumn(ColumnViewModel columnToRemove)
    {
        Columns.Remove(columnToRemove);
    }

    [RelayCommand]
    private async Task Rename()
    {
        var textInputControl = new Views.Dialogs.SimpleTextInputDialog(Name, x => !string.IsNullOrWhiteSpace(x));
        var result = await DialogHost.Show(textInputControl, "RootDialog");

        if (result is not string resultString)
        {
            return;
        }

        Name = resultString;
    }

    [RelayCommand]
    private async Task AddConnection()
    {
        var mv = App.ServiceProvider.GetRequiredService<MainViewModel>();

        var availableTables = mv.Tables.WhereTablesNotConnectedToThis(this);

        var tableConnectorControl = new Views.Dialogs.TableConnectorDialog(availableTables);
        TableViewModel? result = await DialogHost.Show(tableConnectorControl, "RootDialog") as TableViewModel;

        if (result is null)
        {
            return;
        }

        mv.Tables.Add(new LineViewModel(_settings, this, result));
    }

    public void DeleteConnections(ICollection<SelectableElement> connections)
    {
        // Create a list to hold the items to be removed
        var itemsToRemove = new List<LineViewModel>();

        // Iterate over the collection and add items to the removal list
        foreach (LineViewModel line in connections.OfType<LineViewModel>())
        {
            if (line.From == this || line.To == this)
            {
                itemsToRemove.Add(line);
            }
        }

        // Remove the items from the original collection
        foreach (var item in itemsToRemove)
        {
            connections.Remove(item);
        }
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
        angle = MathHelper.NormalizeAngle(angle);

        if (angle.IsBetween(45, 135)) return RelativePosition.Top;
        if (angle.IsBetween(135, 225)) return RelativePosition.Left;
        if (angle.IsBetween(225, 315)) return RelativePosition.Bottom;
        return RelativePosition.Right;
    }
}
