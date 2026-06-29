using Microsoft.Extensions.DependencyInjection;
using ShoeStore.WPF.Services.Interfaces;
using System.Windows;

namespace ShoeStore.WPF.Services;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<Window> _windowStack = new();

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void NavigateTo<TViewModel>() where TViewModel : class
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();

        var viewModelName = typeof(TViewModel).Name;
        var viewName = viewModelName.Replace("ViewModel", "View");

        var viewType = typeof(NavigationService).Assembly
            .GetTypes()
            .FirstOrDefault(t => t.Name == viewName);

        if (viewType == null)
            throw new InvalidOperationException($"View '{viewName}' не найден.");

        var window = (Window)Activator.CreateInstance(viewType)!;
        window.DataContext = viewModel;

        _windowStack.Push(window);
        window.Show();
    }

    public void GoBack()
    {
        if (_windowStack.Count > 1)
        {
            var currentWindow = _windowStack.Pop();
            currentWindow.Close();
        }
    }

    public void CloseCurrentWindow()
    {
        if (_windowStack.TryPop(out var window))
            window.Close();
    }
}