using TgiControl.Models;

namespace TgiControl.Services;

public interface IAuthService
{
    Task<(User User, string Token)> AuthenticateAsync(string username, string password, UserRole role);
    Task<bool> ValidateTokenAsync(string token);
    Task<User> GetCurrentUserAsync(string token);
}

public class DemoAuthService : IAuthService
{
    public Task<(User, string)> AuthenticateAsync(string username, string password, UserRole role)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = $"{username}@tgi.com",
            FirstName = username.Split('.').FirstOrDefault() ?? "Demo",
            LastName = username.Split('.').ElementAtOrDefault(1) ?? "User",
            Role = role,
            Company = "TGI Demo",
            OperationalCenter = "Centro Principal"
        };

        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{user.Id}:{DateTime.UtcNow.Ticks}"));
        return Task.FromResult((user, token));
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        return Task.FromResult(!string.IsNullOrEmpty(token));
    }

    public Task<User> GetCurrentUserAsync(string token)
    {
        return Task.FromResult(new User
        {
            Id = Guid.NewGuid(),
            Username = "demo.user",
            Role = UserRole.Operator
        });
    }
}