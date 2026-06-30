using ShoeStore.Shared.DTOs.Auth;
using ShoeStore.Shared.Helpers;
using ShoeStore.WPF.Commands;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;
using System.Windows;

namespace ShoeStore.WPF.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    private string _login = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;

    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public AsyncRelayCommand LoginCommand { get; }
    public AsyncRelayCommand GuestCommand { get; }

    public Action? CloseAction { get; set; }

    public LoginViewModel(IApiClient apiClient, ISessionService sessionService, INavigationService navigationService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
        _navigationService = navigationService;

        LoginCommand = new AsyncRelayCommand(ExecuteLogin);
        GuestCommand = new AsyncRelayCommand(ExecuteGuestLogin);
    }

    private async Task ExecuteLogin(object? parameter)
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Введите логин и пароль.";
            return;
        }

        IsLoading = true;

        try
        {
            var request = new LoginRequestDto { Login = Login, Password = Password };
            var response = await _apiClient.PostAsync<ApiResponse<LoginResponseDto>>("api/auth/login", request);

            if (response?.Success == true && response.Data != null)
            {
                _sessionService.SetSession(
                    response.Data.Token,
                    response.Data.FullName,
                    response.Data.Role,
                    response.Data.UserId
                );

                _apiClient.SetAuthToken(response.Data.Token);
                _navigationService.ShowProductList();
                CloseAction?.Invoke();
            }
            else
            {
                ErrorMessage = response?.Message ?? "Неверный логин или пароль.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task ExecuteGuestLogin(object? parameter)
    {
        IsLoading = true;

        try
        {
            var response = await _apiClient.PostAsync<ApiResponse<LoginResponseDto>>("api/auth/guest", new { });

            if (response?.Success == true && response.Data != null)
            {
                _sessionService.SetSession(
                    response.Data.Token,
                    response.Data.FullName,
                    response.Data.Role,
                    response.Data.UserId
                );

                _navigationService.ShowProductList();
                CloseAction?.Invoke();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}