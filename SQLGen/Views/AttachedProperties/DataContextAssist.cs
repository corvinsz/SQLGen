using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SQLGen.Views.AttachedProperties;

public static class DataContextAssist
{
	public static readonly DependencyProperty DataContextExProperty =
		DependencyProperty.RegisterAttached("DataContextEx", typeof(object), typeof(DataContextAssist));

	public static object GetDataContextEx(DependencyObject element) => element.GetValue(DataContextExProperty);
	public static void SetDataContextEx(DependencyObject element, object value) => element.SetValue(DataContextExProperty, value);
}
