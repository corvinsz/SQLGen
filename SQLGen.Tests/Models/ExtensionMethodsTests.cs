using SQLGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Tests.Models;
public class ExtensionMethodsTests
{
	public void IsBetween_ReturnsTrue(double value, double min, double max, bool expected)
	{
		//bool actual = value.IsBetween(min, max);
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
