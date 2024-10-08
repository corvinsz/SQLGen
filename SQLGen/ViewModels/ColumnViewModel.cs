﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using SQLGen.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.ViewModels;

public partial class ColumnViewModel : SelectableElement
{
    private readonly TableViewModel _parentTable;

    public static IEnumerable<SqlDbType> SQLTypes { get; } = Enum.GetValues(typeof(SqlDbType)).Cast<SqlDbType>().ToList();

    [ObservableProperty]
    private bool _isPrimaryKey;
    [ObservableProperty]
    private bool _isForeignKey;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private SqlDataType _dataType = new();

    public ColumnViewModel(TableViewModel parentTable)
    {
        _parentTable = parentTable;
    }

    [RelayCommand]
    private void DeleteFromCollection()
    {
        _parentTable?.Columns.Remove(this);
    }

    [RelayCommand]
    private void ToggleForeignKey() => IsForeignKey = !IsForeignKey;

    [RelayCommand]
    private void TogglePrimaryKey() => IsPrimaryKey = !IsPrimaryKey;

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

    internal void PredictTypeAndKey()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return;
        }

        if (string.Equals(Name, "ID", StringComparison.InvariantCultureIgnoreCase))
        {
            this.DataType.Type = SqlDbType.Int;
            IsPrimaryKey = true;
            IsForeignKey = false;
            return;
        }

        if (Name.Contains("FK", StringComparison.InvariantCultureIgnoreCase))
        {
            this.DataType.Type = SqlDbType.Int;
            IsPrimaryKey = false;
            IsForeignKey = true;
            return;
        }
    }
}
