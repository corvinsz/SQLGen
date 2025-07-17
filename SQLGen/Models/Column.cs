using CommunityToolkit.Mvvm.ComponentModel;
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

namespace SQLGen.Models;

public partial class Column : SelectableElement, INameable
{
	private readonly Table _parentTable;

	public static IEnumerable<SqlDbType> SQLTypes { get; } = Enum.GetValues(typeof(SqlDbType)).Cast<SqlDbType>().ToList();

	[ObservableProperty]
	private bool _isPrimaryKey;
	[ObservableProperty]
	private bool _isForeignKey;
	[ObservableProperty]
	private string _name;
	[ObservableProperty]
	private SqlDataType _dataType = new();

	public Column(Table parentTable)
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
