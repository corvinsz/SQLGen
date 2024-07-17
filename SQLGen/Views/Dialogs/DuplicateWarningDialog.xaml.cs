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

namespace SQLGen.Views.Dialogs;

/// <summary>
/// Interaction logic for DuplicateWarningDialog.xaml
/// </summary>
public partial class DuplicateWarningDialog : UserControl
{
	private readonly Action _undoAction;

	public DuplicateWarningDialog(Action undoAction, string message)
	{
		InitializeComponent();
		_undoAction = undoAction ?? throw new ArgumentNullException(nameof(undoAction));
		tbMessage.Text = message;
	}

	private void btnUndo_Click(object sender, RoutedEventArgs e)
	{
		_undoAction.Invoke();
	}
}
