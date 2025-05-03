using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SQLGen.Helpers;
using SQLGen.Services;
using SQLGen.ViewModels;
using SQLGen.Windows;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Velopack;

namespace SQLGen;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	[STAThread]
	private static void Main(string[] args)
	{
		VelopackApp.Build()
					.OnFirstRun((v) => { /* Your first run code here */ })
					//.SetLogger(Log)
					.Run();

		MainAsync(args).GetAwaiter().GetResult();
	}

	public static IServiceProvider Services { get; private set; } = null!;

	private static async Task MainAsync(string[] args)
	{
		using IHost host = CreateHostBuilder(args).Build();
		await host.StartAsync().ConfigureAwait(true);

		Services = host.Services;

		App app = new();
		app.InitializeComponent();
		app.MainWindow = host.Services.GetRequiredService<MainWindow>();
		app.MainWindow.Visibility = Visibility.Visible;
		app.Run();

		await host.StopAsync().ConfigureAwait(true);
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder)
			=> configurationBuilder.AddUserSecrets(typeof(App).Assembly))
		.ConfigureServices((hostContext, services) =>
		{
			// Services
			services.AddSingleton<IThemeService, ThemeService>();
			services.AddSingleton<IInputFileHandler, InputFileHandler>();
			services.AddSingleton<IDialogService, DialogService>();
			services.AddSingleton<IErrorHandler, ErrorHandler>();
			services.AddSingleton<IDuplicateChecker, DuplicateChecker>();

			// Views & ViewModels
			services.AddSingleton<MainWindow>();
			services.AddSingleton<MainWindowViewModel>();
			services.AddSingleton<SettingsView>();
			services.AddSingleton<SettingsViewModel>();
			services.AddSingleton<HomeView>();
			services.AddSingleton<HomeViewModel>();

			services.AddSingleton<WeakReferenceMessenger>();
			services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider => provider.GetRequiredService<WeakReferenceMessenger>());

			services.AddSingleton(_ => Current.Dispatcher);

			services.AddSingleton<ISnackbarMessageQueue>(provider =>
			{
				Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
				return new SnackbarMessageQueue(TimeSpan.FromSeconds(2.0), dispatcher);
			});
		});

	//public static ServiceProvider ServiceProvider { get; private set; }

	//public App()
	//{
	//	var serviceCollection = new ServiceCollection();
	//	ConfigureServices(serviceCollection);
	//	ServiceProvider = serviceCollection.BuildServiceProvider();
	//}

	//private void ConfigureServices(IServiceCollection services)
	//{
	//	services.AddSingleton(_ => Current.Dispatcher);

	//	// Services
	//	services.AddSingleton<IDialogService, DialogService>();
	//	services.AddSingleton<IErrorHandler, ErrorHandler>();
	//	services.AddSingleton<ISnackbarMessageQueue>(provider =>
	//	{
	//		Dispatcher dispatcher = provider.GetRequiredService<Dispatcher>();
	//		return new SnackbarMessageQueue(TimeSpan.FromSeconds(2.0), dispatcher);
	//	});


	//	// ViewModels
	//	services.AddSingleton<MainViewModel>();
	//	services.AddSingleton<MainWindow>();
	//	services.AddSingleton<SettingsViewModel>(sp =>
	//	{
	//		var messageQueue = sp.GetRequiredService<ISnackbarMessageQueue>();
	//		var errorHandler = sp.GetRequiredService<IErrorHandler>();
	//		return new SettingsViewModel("settings.json", messageQueue, errorHandler);
	//	});

	//	services.AddTransient<ExportViewModel>();
	//	services.AddTransient<Table>();
	//}

	//private void OnStartup(object sender, StartupEventArgs e)
	//{
	//	var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
	//	mainWindow.Show();
	//}
}

