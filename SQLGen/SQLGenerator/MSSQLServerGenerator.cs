using SQLGen.ViewModels;
using System.Text;

namespace SQLGen.SQLGenerator;

public class MSSQLServerGenerator : ISQLGenerator
{
	public string Generate(IEnumerable<TableViewModel> tables)
	{
		StringBuilder sql = new();
		foreach (TableViewModel table in tables)
		{
			sql.AppendLine(GenerateTable(table));
			sql.AppendLine();
		}
		return sql.ToString();
	}

	private string GenerateTable(TableViewModel table)
	{
		StringBuilder sql = new();
		sql.AppendLine($"create table {table.Name} (");
		sql.AppendLine(string.Join($",{Environment.NewLine}", table.Columns.Select(x => $"\t{GenerateColumn(x)}")));
		sql.AppendLine($")");
		return sql.ToString();
	}

	private string GenerateColumn(ColumnViewModel column)
	{
		return $"[{column.Name}] {GenerateDataType(column?.DataType)} {GenerateKeyConstraints(column)}";
	}

	private string GenerateDataType(SqlDataType type)
	{
		if (type.HasLength && type.HasPrecision)
		{
			return $"{type.Type}({type.Length},{type.Precision})";
		}
		if (type.HasLength)
		{
			return $"{type.Type}({type.Length})";
		}
		return type.Type.ToString();
	}

	private string GenerateKeyConstraints(ColumnViewModel column)
	{
		if (column.IsPrimaryKey && column.IsForeignKey)
		{
			//TODO
			return "PRIMARY KEY IDENTITY(1,1)";
		}
		if (column.IsPrimaryKey)
		{
			return "PRIMARY KEY IDENTITY(1,1)";
		}
		if (column.IsForeignKey)
		{
			return "FOREIGN KEY REFERENCES <table>(ID)";
		}
		return string.Empty;
	}
}
