using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoeStore.WPF.Services;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels;
using ShoeStore.WPF.Views;
using System.Net.Http;
using System.Windows;

namespace ShoeStore.WPF;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += (s, ex) =>
        {
            MessageBox.Show($"Критическая ошибка: {ex.Exception.Message}\n\n{ex.Exception.StackTrace}");
            ex.Handled = true;
        };

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        ConfigureServices(services, configuration);
        _serviceProvider = services.BuildServiceProvider();

        var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
        var loginView = new LoginView();
        loginView.DataContext = loginViewModel;
        loginViewModel.CloseAction = () => loginView.Close();
        loginView.Show();
    }

    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    var baseUrl = configuration["ApiSettings:BaseUrl"]!;

    services.AddSingleton<HttpClient>(provider =>
    {
        return new HttpClient { BaseAddress = new Uri(baseUrl) };
    });

    services.AddSingleton<IApiClient, ApiClient>();

    services.AddSingleton<ISessionService, SessionService>();
    services.AddSingleton<INavigationService, NavigationService>(provider =>
        new NavigationService(provider));

    services.AddTransient<LoginViewModel>();
    services.AddTransient<ProductListViewModel>();
    services.AddTransient<ProductEditViewModel>();
}

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}