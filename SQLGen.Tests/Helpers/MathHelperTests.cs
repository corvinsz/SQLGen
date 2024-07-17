using Microsoft.VisualStudio.TestPlatform.TestHost;
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


	[Theory]
	[InlineData(5.3, 1.0, 6.0)]  // Simple rounding up
	[InlineData(5.3, 2.0, 6.0)]  // Round up to the nearest 2
	[InlineData(5.3, 0.5, 5.5)]  // Round up to the nearest 0.5
	[InlineData(5.0, 2.0, 6.0)]  // Round an exact multiple up
	[InlineData(0.0, 2.0, 0.0)]  // Zero input
	[InlineData(5.3, 0, 5.3)]    // Zero interval
	public void RoundToNextUpperInterval_ReturnsExpectedValue(double numberToRound, double interval, double expected)
	{
		var result = SQLGen.Helpers.MathHelper.RoundToNextUpperInterval(numberToRound, interval);
		Assert.Equal(expected, result);
	}
}
