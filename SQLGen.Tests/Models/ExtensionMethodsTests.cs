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

    [Fact]
    public void IsEmpty_WithEmptyList_ReturnsTrue()
    {
        // Arrange
        var emptyList = new List<int>();

        // Act
        bool result = emptyList.IsEmpty();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsEmpty_WithNonEmptyList_ReturnsFalse()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3 };

        // Act
        bool result = list.IsEmpty();

        // Assert
        Assert.False(result);
    }
    [Fact]
    public void IsEmpty_WithNullEnumerable_ThrowsArgumentNullException()
    {
        // Arrange
        IEnumerable<int> nullEnumerable = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => nullEnumerable.IsEmpty());
    }
}
