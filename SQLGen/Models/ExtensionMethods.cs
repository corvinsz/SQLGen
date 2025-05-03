using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Models;

public static class ExtensionMethods
{
	public static bool IsBetween(this double value, double min, double max)
	{
		return min <= value && value <= max;
	}

	public static RelativePosition GetOppositeSide(this RelativePosition pos)
	{
		return pos switch
		{
			RelativePosition.Top => RelativePosition.Bottom,
			RelativePosition.Right => RelativePosition.Left,
			RelativePosition.Bottom => RelativePosition.Top,
			RelativePosition.Left => RelativePosition.Right,
			_ => throw new NotImplementedException($"Method {nameof(GetOppositeSide)} is not fully implemented"),
		};
	}

	/// <summary>
	/// Calling "Any()" on an IEnumerable *is* faster than "Count() > 0"
	/// Due to readability I wrapped the Any() in an extension method
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();
}
