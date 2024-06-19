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
        switch (pos)
        {
            case RelativePosition.Top:
                return RelativePosition.Bottom;
            case RelativePosition.Right:
                return RelativePosition.Left;
            case RelativePosition.Bottom:
                return RelativePosition.Top;
            case RelativePosition.Left:
                return RelativePosition.Right;
            default:
                throw new NotImplementedException($"Method {nameof(GetOppositeSide)} is not fully implemented");
        }
    }
}
