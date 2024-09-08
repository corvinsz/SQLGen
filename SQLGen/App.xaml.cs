using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using SQLGen.Helpers;
using SQLGen.ViewModels;
using SQLGen.Windows;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;

namespace SQLGen;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static ServiceProvider ServiceProvider { get; private set; }

    public App()
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<SettingsViewModel>(new SettingsViewModel("settings.json"));
        services.AddSingleton<IMessageService<SnackbarMessageQueue>>(x => new SnackbarMessageService(new SnackbarMessageQueue()));

        services.AddTransient<ExportViewModel>();
        services.AddTransient<TableViewModel>();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = ServiceProvider.GetService<MainWindow>();
        mainWindow.Show();
    }
}

