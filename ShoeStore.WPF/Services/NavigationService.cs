using Microsoft.Extensions.DependencyInjection;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels;
using ShoeStore.WPF.Views;
using System.Windows;

namespace ShoeStore.WPF.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ShowProductList()
    {
        var vm = _serviceProvider.GetRequiredService<ProductListViewModel>();
        var window = new ProductListView();
        window.DataContext = vm;
        window.Show();
    }

    public void ShowLogin()
    {
        var vm = _serviceProvider.GetRequiredService<LoginViewModel>();
        var window = new LoginView();
        window.DataContext = vm;
        window.Show();
    }
}