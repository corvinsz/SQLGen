using Moq;
using SQLGen.SQLGenerator;
using SQLGen.ViewModels;

namespace SQLGen.Tests.SQLGenerator;

public class MSSQLServerGeneratorTests
{
	[Fact]
	public void Generate_GeneratesCreateStatement()
	{
		//Arrange
		ISQLGenerator generator = new SQLGen.SQLGenerator.MSSQLServerGenerator();
		var tables = new List<TableViewModel>();
		var tableA = Mock.Of<TableViewModel>();
		tableA.Name = "Person";
		tableA.Columns.Add(new ColumnViewModel(tableA) { Name = "ID", IsPrimaryKey = true, DataType = new SqlDataType() { Type = System.Data.SqlDbType.Int } });
		tableA.Columns.Add(new ColumnViewModel(tableA) { Name = "FirstName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tableA.Columns.Add(new ColumnViewModel(tableA) { Name = "LastName", DataType = new SqlDataType() { Type = System.Data.SqlDbType.NVarChar, Length = 256 } });
		tableA.Columns.Add(new ColumnViewModel(tableA) { Name = "CreatedAt", DataType = new SqlDataType() { Type = System.Data.SqlDbType.DateTime2 } });
		tables.Add(tableA);

		string expected = @"create table Person (
	[ID] Int PRIMARY KEY IDENTITY(1,1),
	[FirstName] NVarChar(256) ,
	[LastName] NVarChar(256) ,
	[Created] DateTime2
)";

		//Act
		string actual = generator.Generate(tables);

		//string expectedLiteral = Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(expected, false);
		//string actualLiteral = Microsoft.CodeAnalysis.CSharp.SymbolDisplay.FormatLiteral(actual, false);

		//Assert
		Assert.Equal(expected,
					 actual,
					 ignoreCase: true,
					 ignoreLineEndingDifferences: true,
					 ignoreWhiteSpaceDifferences: true,
					 ignoreAllWhiteSpace: true);
	}
}
