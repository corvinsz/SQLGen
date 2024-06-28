using SQLGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Tests.Models;
public class ExtensionMethodsTests
{
	[Theory]
	[InlineData(5, 4, 5, true)]
	[InlineData(5, 10, 11, false)]
	[InlineData(0, 0, 0, true)]
	[InlineData(0, -1, 0, true)]
	public void IsBetween_ReturnsTrue(double value, double min, double max, bool expected)
	{
		bool actual = value.IsBetween(min, max);
		Assert.Equal(expected, actual);
	}


	[Theory]
	[InlineData(RelativePosition.Top, RelativePosition.Bottom)]
	[InlineData(RelativePosition.Right, RelativePosition.Left)]
	[InlineData(RelativePosition.Bottom, RelativePosition.Top)]
	[InlineData(RelativePosition.Left, RelativePosition.Right)]
	public void GetOppositeSide_ValidPositions_ReturnsCorrectOpposite(RelativePosition input, RelativePosition expected)
	{
		// Act
		var result = input.GetOppositeSide();

		// Assert
		Assert.Equal(expected, result);
	}

	[Fact]
	public void GetOppositeSide_InvalidPosition_ThrowsNotImplementedException()
	{
		// Arrange
		var invalidPosition = (RelativePosition)int.MaxValue;

		// Act & Assert
		var exception = Assert.Throws<NotImplementedException>(() => invalidPosition.GetOppositeSide());
	}
}
