using SQLGen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Helpers;
public static class MathHelper
{
    public static double RoundToNearestValue(double numberToRound, double value)
    {
        if (value == 0)
        {
            return numberToRound;
        }

        var res = Math.Round(numberToRound / value) * value;
        return res;
    }

    public static double RoundToNextUpperInterval(double numberToRound, double interval)
    {
        if (interval == 0)
        {
            return numberToRound;
        }

        int ceilingDivisor = (int)Math.Ceiling(numberToRound / interval);

        var res = ceilingDivisor * interval;
        return res;
    }

    internal static double NormalizeAngle(double angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        return angle;
    }
}
