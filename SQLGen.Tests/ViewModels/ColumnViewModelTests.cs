using Moq;
using SQLGen.Models;
using SQLGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Tests.ViewModels;

public class ColumnViewModelTests
{
	private readonly TableViewModel _parentTable;
	private readonly ColumnViewModel _columnViewModel;

	public ColumnViewModelTests()
	{
		_parentTable = new TableViewModel();
		_columnViewModel = new ColumnViewModel(_parentTable);
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
}
