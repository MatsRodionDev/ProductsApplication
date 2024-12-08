using UserService.BLL.Common.Responses;
using UserService.BLL.Models;

namespace UserService.BLL.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Guid> RegisterAsync(User newUser, CancellationToken cancellationToken = default);
        Task<TokenResponse> RefreshAsync(string? refreshToken, CancellationToken cancellationToken = default);
        Task LogoutAsync(Guid userId, CancellationToken cancellationToken = default);
        Task ActivateAsync(Guid userId, int activatePass, CancellationToken cancellationToken = default);
        Task<Guid> GenerateNewActivateCodeAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
