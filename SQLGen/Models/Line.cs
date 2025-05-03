using CommunityToolkit.Mvvm.ComponentModel;
using SQLGen.Models;
using SQLGen.ViewModels;
using System.Windows.Media;

namespace SQLGen.Models;

public partial class Line : SelectableElement
{
	public Table From { get; }
	public Table To { get; }

	public Line(SettingsViewModel settings, Table from, Table to)
	{
		_settings = settings ?? throw new ArgumentNullException(nameof(settings));
		From = from ?? throw new ArgumentNullException(nameof(from));
		To = to ?? throw new ArgumentNullException(nameof(to));

		Stroke = Brushes.DarkGray;
		StrokeThickness = _settings.LineThickness;

		From.VisualPropertyChanged += Table_VisualPropertyChanged;
		To.VisualPropertyChanged += Table_VisualPropertyChanged;
	}

	private void Table_VisualPropertyChanged(object? sender, Table e)
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
	private readonly SettingsViewModel _settings;
}
