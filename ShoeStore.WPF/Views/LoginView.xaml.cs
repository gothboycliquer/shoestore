using ShoeStore.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ShoeStore.WPF.Views;

public partial class LoginView : Window
{
    public LoginView()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.CloseAction = () => Close();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }
}