using SQLGen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Tests.Helpers;
public class MathHelperTests
{
    [Theory]
    [InlineData(120, 50, 100)]
    [InlineData(130, 50, 150)]
    [InlineData(200, 50, 200)]
    [InlineData(200, 0, 200)]
    [InlineData(0, 10, 0)]
    public void RoundToNearestValue_ReturnsExpectedValue(double numberToRound, double value, double expected)
    {
        // Act
        var result = SQLGen.Helpers.MathHelper.RoundToNearestValue(numberToRound, value);

        // Assert
        Assert.Equal(expected, result);
    }
}
