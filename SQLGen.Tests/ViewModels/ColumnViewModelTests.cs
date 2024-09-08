using Moq;
using SQLGen.ViewModels;
using System.Data;

namespace SQLGen.Tests.ViewModels;

public class ColumnViewModelTests
{
	private readonly TableViewModel _parentTable;
	private readonly ColumnViewModel _columnViewModel;

	public ColumnViewModelTests()
	{
		_parentTable = Mock.Of<TableViewModel>();
		_columnViewModel = Mock.Of<ColumnViewModel>();
	}

	[Fact]
	public void DeleteFromCollection_ShouldRemoveColumnFromParentTable()
	{
		// Arrange
		_parentTable.Columns.Add(_columnViewModel);

		// Act
		_columnViewModel.DeleteFromCollectionCommand.Execute(null);

		// Assert
		Assert.DoesNotContain(_columnViewModel, _parentTable.Columns);
	}

	[Fact]
	public void ToggleForeignKey_ShouldToggleIsForeignKeyProperty()
	{
		// Arrange
		var initialIsForeignKey = _columnViewModel.IsForeignKey;

		// Act
		_columnViewModel.ToggleForeignKeyCommand.Execute(null);

		// Assert
		Assert.Equal(!initialIsForeignKey, _columnViewModel.IsForeignKey);
	}

	[Fact]
	public void TogglePrimaryKey_ShouldToggleIsPrimaryKeyProperty()
	{
		// Arrange
		var initialIsPrimaryKey = _columnViewModel.IsPrimaryKey;

		// Act
		_columnViewModel.TogglePrimaryKeyCommand.Execute(null);

		// Assert
		Assert.Equal(!initialIsPrimaryKey, _columnViewModel.IsPrimaryKey);
	}

	[Fact]
	public void PredictTypeAndKey_NameIsNullOrWhiteSpace_ShouldNotChangeProperties()
	{
		// Arrange
		var column = Mock.Of<ColumnViewModel>();
		column.Name = null;
		column.IsPrimaryKey = false;
		column.IsForeignKey = false;

		// Act
		column.PredictTypeAndKey();

		// Assert
		Assert.False(column.IsPrimaryKey);
		Assert.False(column.IsForeignKey);

		// Arrange
		column.Name = " ";
		column.IsPrimaryKey = false;
		column.IsForeignKey = false;

		// Act
		column.PredictTypeAndKey();

		// Assert
		Assert.False(column.IsPrimaryKey);
		Assert.False(column.IsForeignKey);
	}

	[Fact]
	public void PredictTypeAndKey_NameIsID_ShouldSetPrimaryKeyAndType()
	{
		// Arrange
		var column = Mock.Of<ColumnViewModel>();
		column.Name = "ID";
		column.DataType.Type = SqlDbType.Real;

		// Act
		column.PredictTypeAndKey();

		// Assert
		Assert.Equal(SqlDbType.Int, column.DataType.Type);
		Assert.True(column.IsPrimaryKey);
		Assert.False(column.IsForeignKey);
	}

	[Fact]
	public void PredictTypeAndKey_NameContainsFK_ShouldSetForeignKeyAndType()
	{
		// Arrange
		var column = Mock.Of<ColumnViewModel>();
		column.Name = "FK_SomeName";
		column.DataType.Type = SqlDbType.Real;

		// Act
		column.PredictTypeAndKey();

		// Assert
		Assert.Equal(SqlDbType.Int, column.DataType.Type);
		Assert.False(column.IsPrimaryKey);
		Assert.True(column.IsForeignKey);
	}

	[Fact]
	public void PredictTypeAndKey_NameIsOtherValue_ShouldNotChangeProperties()
	{
		// Arrange
		var column = Mock.Of<ColumnViewModel>();
		column.Name = "SomeOtherName";
		column.DataType.Type = SqlDbType.NVarChar;

		// Act
		column.PredictTypeAndKey();

		// Assert
		Assert.Equal(SqlDbType.NVarChar, column.DataType.Type);
		Assert.False(column.IsPrimaryKey);
		Assert.False(column.IsForeignKey);
	}

}
