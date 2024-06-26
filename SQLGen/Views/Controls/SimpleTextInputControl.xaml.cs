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
/// Interaction logic for SimpleTextInputControl.xaml
/// </summary>
public partial class SimpleTextInputControl : UserControl
{
	private readonly Predicate<string> _validator;
	public SimpleTextInputControl(string initialTextValue = null, Predicate<string> validator = null)
	{
		InitializeComponent();
		_validator = validator;
		btnOk.IsEnabled = false;
		tbInput.Text = initialTextValue;

		if (initialTextValue?.Length > 0)
		{
			tbInput.CaretIndex = initialTextValue.Length;
		}
		tbInput.Focus();

		if (validator is null)
		{
			btnOk.IsEnabled = true;
		}
	}

	private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
	{
		if (_validator is null)
		{
			return;
		}

		btnOk.IsEnabled = _validator(tbInput.Text);
	}
}
