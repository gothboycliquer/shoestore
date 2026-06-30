using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;

namespace ShoeStore.WPF.ViewModels;

public class ProductListViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    public ProductListViewModel(IApiClient apiClient, ISessionService sessionService, INavigationService navigationService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
        _navigationService = navigationService;
    }
}