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
}
