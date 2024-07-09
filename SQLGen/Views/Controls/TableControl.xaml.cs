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

    private void ResizeThumbBottom_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
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

    public void ResizeThumbBottom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        DoAutoResize();
    }

    public bool DoAutoResize()
    {
        // Measure the content size
        lbColumns.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        Size contentSize = lbColumns.DesiredSize;

        // Set the UserControl size to fit the content
        double newWidth = contentSize.Width + (lbColumns.BorderThickness.Left + lbColumns.BorderThickness.Right) + (lbColumns.Padding.Left + lbColumns.Padding.Right);
        double newHeight = contentSize.Height + (lbColumns.BorderThickness.Top + lbColumns.BorderThickness.Bottom) + (lbColumns.Padding.Top + lbColumns.Padding.Bottom);

        newWidth = newWidth * 1.2;
        newHeight += headerRow.ActualHeight + footerRow.ActualHeight;

        if (this.Width == newWidth && this.Height == newHeight)
        {
            return false;
        }

        this.Width = newWidth;
        this.Height = newHeight;
        return true;
    }
}
