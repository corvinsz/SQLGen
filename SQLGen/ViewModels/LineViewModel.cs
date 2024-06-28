using CommunityToolkit.Mvvm.ComponentModel;
using SQLGen.Models;
using System.Windows.Media;

namespace SQLGen.ViewModels;

public partial class LineViewModel : SelectableElement
{
	public TableViewModel From { get; }
	public TableViewModel To { get; }

	public LineViewModel(TableViewModel from, TableViewModel to)
	{
		ArgumentNullException.ThrowIfNull(from, nameof(from));
		ArgumentNullException.ThrowIfNull(to, nameof(to));

		Stroke = Brushes.DarkGray;
		StrokeThickness = MainViewModel.Instance.Settings.LineThickness;
		From = from;
		To = to;

		From.VisualPropertyChanged += Table_VisualPropertyChanged;
		To.VisualPropertyChanged += Table_VisualPropertyChanged;
	}

	private void Table_VisualPropertyChanged(object? sender, TableViewModel e)
	{
		CalculateStartAndEndpoint();
	}

	public void CalculateStartAndEndpoint()
	{
		RelativePosition pos = From.CalculateRelativePosition(To);
		System.Windows.Point P1 = From.GetPointOfSide(pos);
		X1 = P1.X;
		Y1 = P1.Y;

		pos = pos.GetOppositeSide();
		System.Windows.Point P2 = To.GetPointOfSide(pos);
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
