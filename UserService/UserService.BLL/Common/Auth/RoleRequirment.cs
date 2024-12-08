using Microsoft.AspNetCore.Authorization;
using Shared.Enums;

namespace UserService.BLL.Common.Auth
{
    public class RoleRequirment : IAuthorizationRequirement
    {
        public RoleRequirment(params RoleEnum[] roles)
        {
            Roles = roles;
        }

        public RoleEnum[] Roles { get; set; } = [];
    }
}
