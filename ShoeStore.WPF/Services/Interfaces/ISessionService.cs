namespace ShoeStore.WPF.Services.Interfaces;

public interface ISessionService
{
    string Token { get; }
    string FullName { get; }
    string Role { get; }
    int UserId { get; }
    bool IsAuthenticated { get; }
    bool IsGuest { get; }

    void SetSession(string token, string fullName, string role, int userId);
    void ClearSession();

    bool IsAdmin();
    bool IsManager();
    bool IsClient();
}