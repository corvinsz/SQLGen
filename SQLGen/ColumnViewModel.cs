using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen;

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
                return $"[{Name}] {DataType.GenerateSQL()} {GenerateKeyConstraints(dbms)}";
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

public partial class SqlDataType : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasLength))]
    [NotifyPropertyChangedFor(nameof(HasPrecision))]
    private SqlDbType _type;

    partial void OnTypeChanged(SqlDbType oldValue, SqlDbType newValue)
    {
        var idk = 2;
    }

    [ObservableProperty]
    private int _length;

    [ObservableProperty]
    private int _precision;

    //Todo add missing types
    private static readonly List<SqlDbType> _typesWithLength = [SqlDbType.Decimal, SqlDbType.Float, SqlDbType.NVarChar];

    //Todo add missing types
    private static readonly List<SqlDbType> _typesWithPrecision = [SqlDbType.Decimal, SqlDbType.Float];

    public bool HasLength => _typesWithLength.Contains(this.Type);
    public bool HasPrecision => _typesWithPrecision.Contains(this.Type);

    public string GenerateSQL()
    {
        if (HasLength && HasPrecision)
        {
            return $"{Type}({Length},{Precision})";
        }
        if (HasLength)
        {
            return $"{Type}({Length})";
        }
        return Type.ToString();
    }
}