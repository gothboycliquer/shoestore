namespace ShoeStore.WPF.Services.Interfaces;

public interface INavigationService
{
    void NavigateTo<TViewModel>() where TViewModel : class;
    void GoBack();
    void CloseCurrentWindow();
}