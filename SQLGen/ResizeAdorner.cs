using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace SQLGen;

public class ResizeAdorner : Adorner
{
    private Thumb thumb;
    private VisualCollection visualChildren;

    public ResizeAdorner(UIElement adornedElement) : base(adornedElement)
    {
        visualChildren = new VisualCollection(this);

        thumb = new Thumb
        {
            Cursor = Cursors.SizeNWSE,
            Width = 10,
            Height = 10,
            Background = new SolidColorBrush(Colors.Green)
        };

        thumb.DragDelta += Thumb_DragDelta;
        visualChildren.Add(thumb);
    }

    private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (this.AdornedElement is FrameworkElement adornedElement)
        {
            adornedElement.Width = Math.Max(adornedElement.Width + e.HorizontalChange, thumb.DesiredSize.Width);
            adornedElement.Height = Math.Max(adornedElement.Height + e.VerticalChange, thumb.DesiredSize.Height);
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        thumb.Arrange(new Rect(finalSize.Width - thumb.DesiredSize.Width, finalSize.Height - thumb.DesiredSize.Height, thumb.DesiredSize.Width, thumb.DesiredSize.Height));
        return finalSize;
    }

    protected override int VisualChildrenCount => visualChildren.Count;

    protected override Visual GetVisualChild(int index)
    {
        return visualChildren[index];
    }
}