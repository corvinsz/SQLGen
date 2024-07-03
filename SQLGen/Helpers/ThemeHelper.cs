using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLGen.Helpers;

public static class ThemeHelper
{
	public static void ToggleTheme(bool isDark)
	{
		var paletteHelper = new PaletteHelper();
		var theme = paletteHelper.GetTheme();

		theme.SetBaseTheme(isDark ? BaseTheme.Dark : BaseTheme.Light);
		paletteHelper.SetTheme(theme);
	}
}
