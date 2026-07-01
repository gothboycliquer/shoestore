using ShoeStore.Shared.DTOs.Orders;
using ShoeStore.Shared.DTOs.Products;
using ShoeStore.Shared.Helpers;
using ShoeStore.WPF.Commands;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace ShoeStore.WPF.ViewModels;

public class OrderEditViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;
    private readonly ISessionService _sessionService;

    private int _id;
    private string _status = "Новый";
    private bool _isEditMode;
    private ProductDto? _selectedProduct;
    private int _selectedQuantity = 1;

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Status
    {
        get => _status;
        set => SetProperty(ref _status, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public ProductDto? SelectedProduct
    {
        get => _selectedProduct;
        set => SetProperty(ref _selectedProduct, value);
    }

    public int SelectedQuantity
    {
        get => _selectedQuantity;
        set => SetProperty(ref _selectedQuantity, value);
    }

    public string WindowTitle => IsEditMode ? "Редактирование заказа" : "Новый заказ";

    public ObservableCollection<string> Statuses { get; } = new()
    {
        "Новый", "В обработке", "Выполнен", "Отменён"
    };

    public ObservableCollection<ProductDto> Products { get; } = new();
    public ObservableCollection<OrderItemDto> OrderItems { get; } = new();

    public AsyncRelayCommand SaveCommand { get; }
    public RelayCommand AddItemCommand { get; }
    public RelayCommand RemoveItemCommand { get; }
    public RelayCommand CancelCommand { get; }

    public Action? CloseAction { get; set; }
    public Action? OnSavedAction { get; set; }

    public OrderEditViewModel(IApiClient apiClient, ISessionService sessionService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;

        SaveCommand = new AsyncRelayCommand(SaveAsync);
        AddItemCommand = new RelayCommand(_ => AddItem());
        RemoveItemCommand = new RelayCommand(p => RemoveItem(p as OrderItemDto));
        CancelCommand = new RelayCommand(_ => CloseAction?.Invoke());
    }

    public async Task InitializeAsync(OrderDto? order = null)
    {
        await LoadProductsAsync();

        if (order != null)
        {
            IsEditMode = true;
            Id = order.Id;
            Status = order.Status;

            foreach (var item in order.OrderItems)
                OrderItems.Add(item);
        }
        else
        {
            IsEditMode = false;
        }
    }

    private async Task LoadProductsAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<IEnumerable<ProductDto>>>("api/products");
        if (response?.Success == true && response.Data != null)
        {
            Products.Clear();
            foreach (var item in response.Data)
                Products.Add(item);
        }
    }

    private void AddItem()
    {
        if (SelectedProduct == null)
        {
            MessageBox.Show("Выберите товар.");
            return;
        }

        if (SelectedQuantity <= 0)
        {
            MessageBox.Show("Количество должно быть больше 0.");
            return;
        }

        var existing = OrderItems.FirstOrDefault(i => i.ProductId == SelectedProduct.Id);
        if (existing != null)
        {
            MessageBox.Show("Этот товар уже добавлен в заказ.");
            return;
        }

        OrderItems.Add(new OrderItemDto
        {
            ProductId = SelectedProduct.Id,
            ProductName = SelectedProduct.Name,
            Quantity = SelectedQuantity,
            PriceAtPurchase = SelectedProduct.Price
        });
    }

    private void RemoveItem(OrderItemDto? item)
    {
        if (item == null) return;
        OrderItems.Remove(item);
    }

    private async Task SaveAsync(object? parameter)
    {
        if (OrderItems.Count == 0)
        {
            MessageBox.Show("Добавьте хотя бы один товар в заказ.");
            return;
        }

        try
        {
            var dto = new CreateOrderDto
            {
                UserId = _sessionService.UserId,
                Status = Status,
                OrderItems = OrderItems.Select(i => new CreateOrderItemDto
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList()
            };

            if (IsEditMode)
            {
                var response = await _apiClient.PutAsync<ApiResponse<OrderDto>>($"api/orders/{Id}", dto);
                if (response?.Success == true)
                {
                    MessageBox.Show("Заказ успешно обновлён.");
                    OnSavedAction?.Invoke();
                    CloseAction?.Invoke();
                }
                else
                {
                    MessageBox.Show(response?.Message ?? "Ошибка при обновлении заказа.");
                }
            }
            else
            {
                var response = await _apiClient.PostAsync<ApiResponse<OrderDto>>("api/orders", dto);
                if (response?.Success == true)
                {
                    MessageBox.Show("Заказ успешно создан.");
                    OnSavedAction?.Invoke();
                    CloseAction?.Invoke();
                }
                else
                {
                    MessageBox.Show(response?.Message ?? "Ошибка при создании заказа.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}