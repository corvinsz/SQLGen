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

namespace SQLGen;
/// <summary>
/// Interaction logic for TableControl.xaml
/// </summary>
public partial class TableControl : UserControl
{
    private AdornerLayer adornerLayer;
    private ResizeAdorner resizeAdorner;
    public TableControl()
    {
        InitializeComponent();
        this.Loaded += MyUserControl_Loaded;
    }

    private void MyUserControl_Loaded(object sender, RoutedEventArgs e)
    {
        // Ensure the adorner layer is available
        adornerLayer = AdornerLayer.GetAdornerLayer(this);

        if (adornerLayer is not null)
        {
            // Create and add the resize adorner
            resizeAdorner = new ResizeAdorner(this);
            adornerLayer.Add(resizeAdorner);
        }
    }

    //private void resizeThumbBottom_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    //{
    //    if (Card is not null)
    //    {
    //        //double newWidth = Card.Width + e.HorizontalChange;
    //        double newHeight = Card.Height + e.VerticalChange;

    //        // Set new width and height with minimum constraints
    //        //Card.Width = newWidth > 0 ? newWidth : 0;
    //        Card.Height = newHeight > 0 ? newHeight : 0;
    //    }
    //}

    //private void resizeThumbRight_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
    //{
    //    if (Card is not null)
    //    {
    //        //double newWidth = Card.Width + e.HorizontalChange;
    //        double newHeight = Card.Height + e.VerticalChange;

    //        // Set new width and height with minimum constraints
    //        //Card.Width = newWidth > 0 ? newWidth : 0;
    //        Card.Height = newHeight > 0 ? newHeight : 0;
    //    }
    //}
}
