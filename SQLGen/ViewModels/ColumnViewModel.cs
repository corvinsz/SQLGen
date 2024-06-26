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
	private SqlDataType _dataType;

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



	internal string? GenerateSQL(DBMS dbms)
	{
		switch (dbms)
		{
			case DBMS.MySQL:
				break;
			case DBMS.MariaDB:
				break;
			case DBMS.MSSQLServer:
				return $"[{Name}] {DataType?.GenerateSQL()} {GenerateKeyConstraints(dbms)}";
		}
		throw new NotImplementedException();
	}

	private string GenerateKeyConstraints(DBMS dbms)
	{
		if (IsPrimaryKey && IsForeignKey)
		{
			//TODO
			return "PRIMARY KEY IDENTITY(1,1)";
		}
		if (IsPrimaryKey)
		{
			return "PRIMARY KEY IDENTITY(1,1)";
		}
		if (IsForeignKey)
		{
			return "FOREIGN KEY REFERENCES <table>(ID)";
		}
		return string.Empty;
	}
}
