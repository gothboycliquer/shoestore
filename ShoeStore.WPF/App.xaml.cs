using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoeStore.WPF.Services;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels;
using ShoeStore.WPF.Views;
using System.Windows;

namespace ShoeStore.WPF;

public partial class App : Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var services = new ServiceCollection();
        ConfigureServices(services, configuration);
        _serviceProvider = services.BuildServiceProvider();

        var loginView = _serviceProvider.GetRequiredService<LoginView>();
        var loginViewModel = _serviceProvider.GetRequiredService<LoginViewModel>();
        loginView.DataContext = loginViewModel;
        loginView.Show();
    }

    private void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["ApiSettings:BaseUrl"]!;
        services.AddHttpClient<IApiClient, ApiClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
        });

        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<INavigationService, NavigationService>(provider =>
            new NavigationService(provider));

        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<ProductListViewModel>();

        services.AddTransient<LoginView>();
        services.AddTransient<MainView>();
        services.AddTransient<ProductListView>();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}