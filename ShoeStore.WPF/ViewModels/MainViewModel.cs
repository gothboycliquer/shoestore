using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;

namespace ShoeStore.WPF.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    private string _welcomeMessage = string.Empty;

    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set => SetProperty(ref _welcomeMessage, value);
    }

    public MainViewModel(ISessionService sessionService, INavigationService navigationService)
    {
        _sessionService = sessionService;
        _navigationService = navigationService;
        WelcomeMessage = $"Добро пожаловать, {_sessionService.FullName}!";
    }
}