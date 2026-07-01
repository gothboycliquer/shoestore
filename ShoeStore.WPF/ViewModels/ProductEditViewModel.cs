using Microsoft.Win32;
using ShoeStore.Shared.DTOs.Categories;
using ShoeStore.Shared.DTOs.Manufacturers;
using ShoeStore.Shared.DTOs.Products;
using ShoeStore.Shared.DTOs.Suppliers;
using ShoeStore.Shared.Helpers;
using ShoeStore.WPF.Commands;
using ShoeStore.WPF.Services.Interfaces;
using ShoeStore.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace ShoeStore.WPF.ViewModels;

public class ProductEditViewModel : BaseViewModel
{
    private readonly IApiClient _apiClient;

    private int _id;
    private string _name = string.Empty;
    private string _description = string.Empty;
    private decimal _price;
    private string _unit = string.Empty;
    private int _quantity;
    private decimal _discount;
    private string? _imagePath;
    private CategoryDto? _selectedCategory;
    private ManufacturerDto? _selectedManufacturer;
    private SupplierDto? _selectedSupplier;
    private bool _isEditMode;

    public int Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public decimal Price
    {
        get => _price;
        set => SetProperty(ref _price, value);
    }

    public string Unit
    {
        get => _unit;
        set => SetProperty(ref _unit, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => SetProperty(ref _quantity, value);
    }

    public decimal Discount
    {
        get => _discount;
        set => SetProperty(ref _discount, value);
    }

    public string? ImagePath
    {
        get => _imagePath;
        set => SetProperty(ref _imagePath, value);
    }

    public CategoryDto? SelectedCategory
    {
        get => _selectedCategory;
        set => SetProperty(ref _selectedCategory, value);
    }

    public ManufacturerDto? SelectedManufacturer
    {
        get => _selectedManufacturer;
        set => SetProperty(ref _selectedManufacturer, value);
    }

    public SupplierDto? SelectedSupplier
    {
        get => _selectedSupplier;
        set => SetProperty(ref _selectedSupplier, value);
    }

    public bool IsEditMode
    {
        get => _isEditMode;
        set => SetProperty(ref _isEditMode, value);
    }

    public string WindowTitle => IsEditMode ? "Редактирование товара" : "Добавление товара";

    public ObservableCollection<CategoryDto> Categories { get; } = new();
    public ObservableCollection<ManufacturerDto> Manufacturers { get; } = new();
    public ObservableCollection<SupplierDto> Suppliers { get; } = new();

    public AsyncRelayCommand SaveCommand { get; }
    public RelayCommand SelectImageCommand { get; }
    public RelayCommand CancelCommand { get; }

    public Action? CloseAction { get; set; }
    public Action? OnSavedAction { get; set; }

    public ProductEditViewModel(IApiClient apiClient)
    {
        _apiClient = apiClient;

        SaveCommand = new AsyncRelayCommand(SaveAsync);
        SelectImageCommand = new RelayCommand(_ => SelectImage());
        CancelCommand = new RelayCommand(_ => CloseAction?.Invoke());
    }

    public async Task InitializeAsync(ProductDto? product = null)
    {
        await LoadCategoriesAsync();
        await LoadManufacturersAsync();
        await LoadSuppliersAsync();

        if (product != null)
        {
            IsEditMode = true;
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Unit = product.Unit;
            Quantity = product.Quantity;
            Discount = product.Discount;
            ImagePath = product.ImagePath;

            SelectedCategory = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
            SelectedManufacturer = Manufacturers.FirstOrDefault(m => m.Id == product.ManufacturerId);
            SelectedSupplier = Suppliers.FirstOrDefault(s => s.Id == product.SupplierId);
        }
        else
        {
            IsEditMode = false;
            Unit = "пара";
        }
    }

    private async Task LoadCategoriesAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<IEnumerable<CategoryDto>>>("api/categories");
        if (response?.Success == true && response.Data != null)
        {
            Categories.Clear();
            foreach (var item in response.Data)
                Categories.Add(item);
        }
    }

    private async Task LoadManufacturersAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<IEnumerable<ManufacturerDto>>>("api/manufacturers");
        if (response?.Success == true && response.Data != null)
        {
            Manufacturers.Clear();
            foreach (var item in response.Data)
                Manufacturers.Add(item);
        }
    }

    private async Task LoadSuppliersAsync()
    {
        var response = await _apiClient.GetAsync<ApiResponse<IEnumerable<SupplierDto>>>("api/suppliers");
        if (response?.Success == true && response.Data != null)
        {
            Suppliers.Clear();
            foreach (var item in response.Data)
                Suppliers.Add(item);
        }
    }

    private void SelectImage()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Выберите изображение",
            Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp",
            Multiselect = false
        };

        if (dialog.ShowDialog() != true) return;

        var sourcePath = dialog.FileName;
        var imagesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

        if (!Directory.Exists(imagesFolder))
            Directory.CreateDirectory(imagesFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(sourcePath)}";
        var destPath = Path.Combine(imagesFolder, fileName);

        if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
            File.Delete(ImagePath);

        File.Copy(sourcePath, destPath);
        ImagePath = destPath;
    }

    private async Task SaveAsync(object? parameter)
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            MessageBox.Show("Введите название товара.");
            return;
        }

        if (SelectedCategory == null)
        {
            MessageBox.Show("Выберите категорию.");
            return;
        }

        if (SelectedManufacturer == null)
        {
            MessageBox.Show("Выберите производителя.");
            return;
        }

        if (SelectedSupplier == null)
        {
            MessageBox.Show("Выберите поставщика.");
            return;
        }

        if (Price < 0)
        {
            MessageBox.Show("Цена не может быть отрицательной.");
            return;
        }

        if (Quantity < 0)
        {
            MessageBox.Show("Количество не может быть отрицательным.");
            return;
        }

        if (Discount < 0 || Discount > 100)
        {
            MessageBox.Show("Скидка должна быть от 0 до 100.");
            return;
        }

        try
        {
            if (IsEditMode)
            {
                var dto = new UpdateProductDto
                {
                    Id = Id,
                    Name = Name,
                    Description = Description,
                    Price = Price,
                    Unit = Unit,
                    Quantity = Quantity,
                    Discount = Discount,
                    ImagePath = ImagePath,
                    CategoryId = SelectedCategory.Id,
                    ManufacturerId = SelectedManufacturer.Id,
                    SupplierId = SelectedSupplier.Id
                };

                var response = await _apiClient.PutAsync<ApiResponse<ProductDto>>($"api/products/{Id}", dto);

                if (response?.Success == true)
                {
                    MessageBox.Show("Товар успешно обновлён.");
                    OnSavedAction?.Invoke();
                    CloseAction?.Invoke();
                }
                else
                {
                    MessageBox.Show(response?.Message ?? "Ошибка при обновлении товара.");
                }
            }
            else
            {
                var dto = new CreateProductDto
                {
                    Name = Name,
                    Description = Description,
                    Price = Price,
                    Unit = Unit,
                    Quantity = Quantity,
                    Discount = Discount,
                    ImagePath = ImagePath,
                    CategoryId = SelectedCategory.Id,
                    ManufacturerId = SelectedManufacturer.Id,
                    SupplierId = SelectedSupplier.Id
                };

                var response = await _apiClient.PostAsync<ApiResponse<ProductDto>>("api/products", dto);

                if (response?.Success == true)
                {
                    MessageBox.Show("Товар успешно добавлен.");
                    OnSavedAction?.Invoke();
                    CloseAction?.Invoke();
                }
                else
                {
                    MessageBox.Show(response?.Message ?? "Ошибка при добавлении товара.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка: {ex.Message}");
        }
    }
}