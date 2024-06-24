using CommunityToolkit.Mvvm.ComponentModel;
using SQLGen.Models;
using System.Windows.Media;

namespace SQLGen.ViewModels;

public partial class LineViewModel : SelectableElement
{
	private readonly TableViewModel _from;
	private readonly TableViewModel _to;

	public LineViewModel(TableViewModel from, TableViewModel to)
	{
		ArgumentNullException.ThrowIfNull(from, nameof(from));
		ArgumentNullException.ThrowIfNull(to, nameof(to));

		Stroke = Brushes.DarkGray;
		StrokeThickness = 2;
		_from = from;
		_to = to;

		_from.VisualPropertyChanged += Table_VisualPropertyChanged;
		_to.VisualPropertyChanged += Table_VisualPropertyChanged;
	}

	private void Table_VisualPropertyChanged(object? sender, TableViewModel e)
	{
		CalculatePositions();
	}

	private void CalculatePositions()
	{
		RelativePosition pos = _from.CalculateRelativePosition(_to);
		System.Windows.Point P1 = _from.GetPointOfSide(pos);
		X1 = P1.X;
		Y1 = P1.Y;

		pos = pos.GetOppositeSide();
		System.Windows.Point P2 = _to.GetPointOfSide(pos);
		X2 = P2.X;
		Y2 = P2.Y;
	}

	[ObservableProperty]
	private double _x1;
	[ObservableProperty]
	private double _y1;
	[ObservableProperty]
	private double _x2;
	[ObservableProperty]
	private double _y2;

	[ObservableProperty]
	private Brush _stroke;

	[ObservableProperty]
	private double _strokeThickness;
}
