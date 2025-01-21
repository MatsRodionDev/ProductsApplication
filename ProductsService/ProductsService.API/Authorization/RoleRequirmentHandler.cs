using Microsoft.AspNetCore.Authorization;
using Shared.Consts;
using Shared.Enums;


namespace ProductsService.API.Authorization
{
    public class RoleRequirmentHandler : AuthorizationHandler<RoleRequirment>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            RoleRequirment requirement)
        {
            var userRole = context.User.Claims.FirstOrDefault(
                 c => c.Type == CustomClaims.USER_ROLE_CLAIM_KEY);

            if (userRole == null || !Enum.TryParse<RoleEnum>(userRole.Value, out var role))
            {
                return Task.CompletedTask;
            }

            if (requirement.Roles.Contains(role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}

