﻿using Microsoft.AspNetCore.Authorization;
using Shared.Enums;

namespace UserService.API.Authorization
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