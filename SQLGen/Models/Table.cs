using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SQLGen.Helpers;
using SQLGen.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Text;
using System.Windows;

namespace SQLGen.Models;

public partial class Table : SelectableElement, INameable
{
	[ObservableProperty]
	private string _name;

	public ObservableCollection<Column> Columns { get; } = [];

	//Visual Properties
	[ObservableProperty]
	private double _x;
	[ObservableProperty]
	private double _y;
	[ObservableProperty]
	private double _height;
	[ObservableProperty]
	private double _width;

	partial void OnXChanged(double value)
	{
		//Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		_x = Helpers.MathHelper.RoundToNearestValue(value, /* TODO */ 8);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		VisualPropertyChanged?.Invoke(this, this);
	}
	partial void OnYChanged(double value)
	{
		//Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		_y = Helpers.MathHelper.RoundToNearestValue(value, /* TODO */ 8);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		VisualPropertyChanged?.Invoke(this, this);
	}
	partial void OnHeightChanged(double value)
	{
		//Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		_height = Helpers.MathHelper.RoundToNextUpperInterval(value, /* TODO */ 8);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		VisualPropertyChanged?.Invoke(this, this);
	}

	partial void OnWidthChanged(double value)
	{
		//Setting the field is ok, otherwise a stackoverflowexception would be thrown
#pragma warning disable MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		_width = Helpers.MathHelper.RoundToNextUpperInterval(value, /* TODO */ 8);
#pragma warning restore MVVMTK0034 // Direct field reference to [ObservableProperty] backing field
		VisualPropertyChanged?.Invoke(this, this);
	}

	public event EventHandler<Table> VisualPropertyChanged;

	public void DeleteConnections(ICollection<SelectableElement> connections)
	{
		var itemsToRemove = new List<Line>();

		foreach (Line line in connections.OfType<Line>())
		{
			if (line.From == this || line.To == this)
			{
				itemsToRemove.Add(line);
			}
		}

		foreach (var item in itemsToRemove)
		{
			connections.Remove(item);
		}
	}

	internal RelativePosition CalculateRelativePosition(Table to)
	{
		double centerX1 = this.X + (this.Width / 2);
		double centerY1 = this.Y + (this.Height / 2);
		double centerX2 = to.X + (to.Width / 2);
		double centerY2 = to.Y + (to.Height / 2);

		double deltaX = centerX2 - centerX1;
		double deltaY = centerY2 - centerY1;

		// Invert deltaY to account for the top-left origin of the WPF coordinate system
		deltaY = -deltaY;

		//Calculate angle
		double angleInDegrees = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

		//Convert angle to enum
		return DegreeToRelativePosition(angleInDegrees);
	}

	internal Point GetPointOfSide(RelativePosition side)
	{
		return side switch
		{
			RelativePosition.Top => new Point((X + Width / 2), Y),
			RelativePosition.Right => new Point((X + Width), (Y + Height / 2)),
			RelativePosition.Bottom => new Point((X + Width / 2), Y + Height),
			RelativePosition.Left => new Point(X, Y + Height / 2),
			_ => throw new NotImplementedException($"Method {nameof(GetPointOfSide)} is not fully implemented"),
		};
	}

	private static RelativePosition DegreeToRelativePosition(double angle)
	{
		angle = MathHelper.NormalizeAngle(angle);

		if (angle.IsBetween(45, 135)) return RelativePosition.Top;
		if (angle.IsBetween(135, 225)) return RelativePosition.Left;
		if (angle.IsBetween(225, 315)) return RelativePosition.Bottom;
		return RelativePosition.Right;
	}
}
