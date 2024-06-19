using System.Configuration;
using System.Data;
using System.Globalization;
using System.Windows;

namespace SQLGen;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
    }
}

