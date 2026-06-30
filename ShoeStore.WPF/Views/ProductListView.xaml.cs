using ShoeStore.WPF.ViewModels;
using System.Windows;

namespace ShoeStore.WPF.Views;

public partial class ProductListView : Window
{
    public ProductListView()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        try
        {
            if (DataContext is ProductListViewModel vm)
                await vm.InitializeAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при загрузке: {ex.Message}\n\n{ex.StackTrace}");
        }
    }
}