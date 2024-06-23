using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLGen.Views.Controls;
/// <summary>
/// Interaction logic for TableControl.xaml
/// </summary>
public partial class TableControl : UserControl
{
	private const int MIN_SIZE = 40;
	public TableControl()
	{
		InitializeComponent();
	}

	private void resizeThumbBottom_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
	{
		if (thisControl.Height > MIN_SIZE)
		{
			thisControl.Height += e.VerticalChange;
		}

		if (thisControl.Width > MIN_SIZE)
		{
			thisControl.Width += e.HorizontalChange;
		}

		e.Handled = true;
	}

	private void thisControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
	{
		MessageBox.Show("lmb");
	}
}
