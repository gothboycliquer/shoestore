using ShoeStore.Shared.DTOs.Orders;
using ShoeStore.Shared.Helpers;
using ShoeStore.WPF.Commands;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace ShoeStore.WPF.ViewModels;

public class OrderListViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    private ObservableCollection<OrderDto> _orders = new();
    private bool _isLoading;

    public ObservableCollection<OrderDto> Orders
    {
        get => _orders;
        set => SetProperty(ref _orders, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool IsAdmin => _sessionService.IsAdmin();

    public AsyncRelayCommand LoadOrdersCommand { get; }
    public RelayCommand AddOrderCommand { get; }
    public RelayCommand EditOrderCommand { get; }
    public AsyncRelayCommand DeleteOrderCommand { get; }
    public RelayCommand GoBackCommand { get; }

    public OrderListViewModel(IApiClient apiClient, ISessionService sessionService, INavigationService navigationService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
        _navigationService = navigationService;

        LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
        AddOrderCommand = new RelayCommand(_ => AddOrder());
        EditOrderCommand = new RelayCommand(p => EditOrder(p as OrderDto));
        DeleteOrderCommand = new AsyncRelayCommand(p => DeleteOrderAsync(p as OrderDto));
        GoBackCommand = new RelayCommand(_ => GoBack());
    }

    public async Task InitializeAsync()
    {
        await LoadOrdersAsync(null);
    }

    private async Task LoadOrdersAsync(object? parameter)
    {
        IsLoading = true;

        try
        {
            var response = await _apiClient.GetAsync<ApiResponse<IEnumerable<OrderDto>>>("api/orders");
            if (response?.Success == true && response.Data != null)
                Orders = new ObservableCollection<OrderDto>(response.Data);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки заказов: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async void AddOrder()
    {
        var vm = new OrderEditViewModel(_apiClient, _sessionService);
        var window = new Views.OrderEditView();
        window.DataContext = vm;
        vm.CloseAction = () => window.Close();
        vm.OnSavedAction = async () => await LoadOrdersAsync(null);
        await vm.InitializeAsync();
        window.ShowDialog();
    }

    private async void EditOrder(OrderDto? order)
    {
        if (order == null) return;

        var vm = new OrderEditViewModel(_apiClient, _sessionService);
        var window = new Views.OrderEditView();
        window.DataContext = vm;
        vm.CloseAction = () => window.Close();
        vm.OnSavedAction = async () => await LoadOrdersAsync(null);
        await vm.InitializeAsync(order);
        window.ShowDialog();
    }

    private async Task DeleteOrderAsync(object? parameter)
    {
        if (parameter is not OrderDto order) return;

        var result = MessageBox.Show(
            $"Удалить заказ №{order.Id}?",
            "Подтверждение",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            var success = await _apiClient.DeleteAsync($"api/orders/{order.Id}");
            if (success)
            {
                Orders.Remove(order);
                MessageBox.Show("Заказ успешно удалён.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка удаления: {ex.Message}");
        }
    }
private void GoBack()
    {
        var existingProductList = Application.Current.Windows
            .OfType<Views.ProductListView>()
            .FirstOrDefault();

        if (existingProductList != null)
        {
            existingProductList.Show();
        }
        else
        {
            _navigationService.ShowProductList();
        }

        foreach (Window window in Application.Current.Windows.OfType<Window>().ToList())
        {
            if (window is not Views.ProductListView)
                window.Close();
        }
    }
}