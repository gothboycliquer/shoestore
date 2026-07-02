using ShoeStore.WPF.Services.Interfaces;

namespace ShoeStore.WPF.Services;

public class SessionService : ISessionService
{
    private string _token = string.Empty;
    private string _fullName = string.Empty;
    private string _role = string.Empty;
    private int _userId;

    public string Token => _token;
    public string FullName => _fullName;
    public string Role => _role;
    public int UserId => _userId;
    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);
    public bool IsGuest => _role == "Гость";

    public void SetSession(string token, string fullName, string role, int userId)
    {
        _token = token;
        _fullName = fullName;
        _role = role;
        _userId = userId;
    }

    public void ClearSession()
    {
        _token = string.Empty;
        _fullName = string.Empty;
        _role = string.Empty;
        _userId = 0;
    }

    public bool IsAdmin() => _role == "Администратор";
    public bool IsManager() => _role == "Менеджер" || IsAdmin();
    public bool IsClient() => _role == "Клиент";
}