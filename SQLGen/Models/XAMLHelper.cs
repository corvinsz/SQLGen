using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SQLGen.Models;
public static class XAMLHelper
{
	public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
	{
		if (depObj == null) yield return (T)Enumerable.Empty<T>();
		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
		{
			DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
			if (ithChild == null) continue;
			if (ithChild is T t) yield return t;
			foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
		}
	}

	public static T FindChild<T>(this DependencyObject depObj)
	where T : DependencyObject
	{
		if (depObj == null) return null;

		for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
		{
			var child = VisualTreeHelper.GetChild(depObj, i);

			var result = (child as T) ?? FindChild<T>(child);
			if (result != null) return result;
		}
		return null;
	}

	public static T FindChild2<T>(DependencyObject parent) where T : DependencyObject
	{
		if (parent == null) return null;

		T foundChild = null;

		int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
		for (int i = 0; i < childrenCount; i++)
		{
			var child = VisualTreeHelper.GetChild(parent, i);

			if (child != null && child is T)
			{
				foundChild = (T)child;
				break;
			}
			else
			{
				foundChild = FindChild2<T>(child);
				if (foundChild != null)
					break;
			}
		}

		return foundChild;
	}
}