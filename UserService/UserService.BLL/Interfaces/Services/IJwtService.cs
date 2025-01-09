using UserService.BLL.Models;

namespace UserService.BLL.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateAccesToken(User user, string role);
        string GenerateRefreshToken(Guid userId);
        IDictionary<string, object>? GetClaimsFromToken(string token);
    }
}
