using ShoeStore.WPF.ViewModels;
using System.Windows;

namespace ShoeStore.WPF.Views;

public partial class OrderListView : Window
{
    public OrderListView()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is OrderListViewModel vm)
            await vm.InitializeAsync();
    }
}