using Shared.Consts;
using System.Security.Claims;

namespace UserService.API.Extensions
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.FindFirst(CustomClaims.USER_ID_CLAIM_KEY)!.Value);
        }
    }
}
