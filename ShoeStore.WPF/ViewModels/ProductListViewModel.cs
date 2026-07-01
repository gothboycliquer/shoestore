using ShoeStore.Shared.DTOs.Products;
using ShoeStore.Shared.DTOs.Suppliers;
using ShoeStore.Shared.Helpers;
using ShoeStore.WPF.Commands;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace ShoeStore.WPF.ViewModels;

public class ProductListViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;
    private readonly ISessionService _sessionService;
    private readonly INavigationService _navigationService;

    private ObservableCollection<ProductDto> _products = new();
    private CollectionViewSource _productsViewSource = new();
    private string _searchText = string.Empty;
    private SupplierDto? _selectedSupplier;
    private ObservableCollection<SupplierDto> _suppliers = new();
    private bool _isLoading;

    public ICollectionView ProductsView => _productsViewSource.View;

    public ObservableCollection<ProductDto> Products
    {
        get => _products;
        set => SetProperty(ref _products, value);
    }

    public ObservableCollection<SupplierDto> Suppliers
    {
        get => _suppliers;
        set => SetProperty(ref _suppliers, value);
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            ProductsView?.Refresh();
        }
    }

    public SupplierDto? SelectedSupplier
    {
        get => _selectedSupplier;
        set
        {
            SetProperty(ref _selectedSupplier, value);
            ProductsView?.Refresh();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public bool IsAdmin => _sessionService.IsAdmin();
    public bool IsManager => _sessionService.IsManager();
    public bool IsGuest => _sessionService.IsGuest;

    public string WelcomeText => $"{_sessionService.FullName} ({_sessionService.Role})";

    public AsyncRelayCommand LoadDataCommand { get; }
    public RelayCommand SortByQuantityAscCommand { get; }
    public RelayCommand SortByQuantityDescCommand { get; }
    public RelayCommand LogoutCommand { get; }
    public RelayCommand AddProductCommand { get; }
    public RelayCommand EditProductCommand { get; }
    public AsyncRelayCommand DeleteProductCommand { get; }
    public RelayCommand ShowOrdersCommand { get; }

    public ProductListViewModel(IApiClient apiClient, ISessionService sessionService, INavigationService navigationService)
    {
        _apiClient = apiClient;
        _sessionService = sessionService;
        _navigationService = navigationService;

        _productsViewSource.Filter += OnFilter;

        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        SortByQuantityAscCommand = new RelayCommand(_ => SortByQuantity("Ascending"));
        SortByQuantityDescCommand = new RelayCommand(_ => SortByQuantity("Descending"));
        LogoutCommand = new RelayCommand(_ => Logout());
        AddProductCommand = new RelayCommand(_ => AddProduct());
        EditProductCommand = new RelayCommand(p => EditProduct(p as ProductDto));
        DeleteProductCommand = new AsyncRelayCommand(p => DeleteProductAsync(p as ProductDto));
        ShowOrdersCommand = new RelayCommand(_ => ShowOrders());
    }

    public async Task InitializeAsync()
    {
        await LoadDataAsync(null);
    }

    private async Task LoadDataAsync(object? parameter)
    {
        IsLoading = true;

        try
        {
            var productsResponse = await _apiClient.GetAsync<ApiResponse<IEnumerable<ProductDto>>>("api/products");
            if (productsResponse?.Success == true && productsResponse.Data != null)
            {
                _products = new ObservableCollection<ProductDto>(productsResponse.Data);
                _productsViewSource.Source = _products;
                OnPropertyChanged(nameof(ProductsView));
            }

            if (IsManager || IsAdmin)
            {
                var suppliersResponse = await _apiClient.GetAsync<ApiResponse<IEnumerable<SupplierDto>>>("api/suppliers");
                if (suppliersResponse?.Success == true && suppliersResponse.Data != null)
                {
                    Suppliers = new ObservableCollection<SupplierDto>(suppliersResponse.Data);
                    Suppliers.Insert(0, new SupplierDto { Id = 0, Name = "Все поставщики" });
                    SelectedSupplier = Suppliers[0];
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void OnFilter(object sender, FilterEventArgs e)
    {
        if (e.Item is not ProductDto product)
        {
            e.Accepted = false;
            return;
        }

        bool matchesSearch = true;
        bool matchesSupplier = true;

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var search = SearchText.ToLower();
            matchesSearch =
                product.Name.ToLower().Contains(search) ||
                product.Description.ToLower().Contains(search) ||
                product.CategoryName.ToLower().Contains(search) ||
                product.ManufacturerName.ToLower().Contains(search) ||
                product.SupplierName.ToLower().Contains(search);
        }

        if (SelectedSupplier != null && SelectedSupplier.Id != 0)
            matchesSupplier = product.SupplierId == SelectedSupplier.Id;

        e.Accepted = matchesSearch && matchesSupplier;
    }

    private void SortByQuantity(string direction)
    {
        ProductsView.SortDescriptions.Clear();

        var sortDir = direction == "Ascending"
            ? ListSortDirection.Ascending
            : ListSortDirection.Descending;

        ProductsView.SortDescriptions.Add(new SortDescription("Quantity", sortDir));
    }

    private async void AddProduct()
{
    var vm = new ProductEditViewModel(_apiClient);
    var window = new Views.ProductEditView();
    window.DataContext = vm;
    vm.CloseAction = () => window.Close();
    vm.OnSavedAction = async () => await LoadDataAsync(null);
    await vm.InitializeAsync();
    window.ShowDialog();
}



private async void EditProduct(ProductDto? product)
{
    if (product == null) return;

    var vm = new ProductEditViewModel(_apiClient);
    var window = new Views.ProductEditView();
    window.DataContext = vm;
    vm.CloseAction = () => window.Close();
    vm.OnSavedAction = async () => await LoadDataAsync(null);
    await vm.InitializeAsync(product);
    window.ShowDialog();
}

    private async Task DeleteProductAsync(object? parameter)
{
    if (parameter is not ProductDto product) return;

    var result = MessageBox.Show(
        $"Удалить товар '{product.Name}'?",
        "Подтверждение",
        MessageBoxButton.YesNo,
        MessageBoxImage.Warning);

    if (result != MessageBoxResult.Yes) return;

    try
    {
        var success = await _apiClient.DeleteAsync($"api/products/{product.Id}");
        if (success)
        {
            _products.Remove(product);
            ProductsView.Refresh();
            MessageBox.Show("Товар успешно удалён.");
        }
        else
        {
            MessageBox.Show("Не удалось удалить товар.");
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Ошибка удаления: {ex.Message}");
    }
}

private void Logout()
    {
        _sessionService.ClearSession();
        _apiClient.ClearAuthToken();
        _navigationService.ShowLogin();

        foreach (Window window in Application.Current.Windows.OfType<Window>().ToList())
        {
            if (window is not Views.LoginView)
                window.Close();
        }
    }
private void ShowOrders()
    {
        var vm = new OrderListViewModel(_apiClient, _sessionService, _navigationService);
        var window = new Views.OrderListView();
        window.DataContext = vm;

        var currentWindow = Application.Current.Windows
        .OfType<Window>()
        .FirstOrDefault(w => w.IsActive);

        window.Show();
        currentWindow?.Hide();
    }
}